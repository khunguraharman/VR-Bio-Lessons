using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using System.IO;
using SlimUI.ModernMenu;
using UnityEngine.InputSystem.XR;
using System.Text;
using System;
using System.Threading.Tasks;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;


public class AllXRData : MonoBehaviour
{
    static string userName = System.Environment.UserName;
    public static string user_session { get; private set; }
    public StreamWriter writeXRdata;
    
    private XRNode[] Nodes = new XRNode[5] {XRNode.CenterEye, XRNode.LeftEye, XRNode.RightEye, XRNode.LeftHand, XRNode.RightHand}; 
    private InputDevice[] Devices = new InputDevice[5];
    private InputFeatureUsage<Vector3>[] Positions = new InputFeatureUsage<Vector3>[5] { CommonUsages.centerEyePosition, CommonUsages.leftEyePosition, CommonUsages.rightEyePosition, CommonUsages.devicePosition, CommonUsages.devicePosition };
    private InputFeatureUsage<Quaternion>[] Rotations = new InputFeatureUsage<Quaternion>[5] { CommonUsages.centerEyeRotation, CommonUsages.leftEyeRotation, CommonUsages.rightEyeRotation, CommonUsages.deviceRotation, CommonUsages.deviceRotation };
    private Vector3[] Position_Values = new Vector3[5];
    private Quaternion[] Rotation_Values = new Quaternion[5];
    public AmazonKinesisFirehoseClient firhoseclient { get; private set; }
    public RegionEndpoint region { get; private set; }
    private AmazonS3Client s3Client;
    public static S3Bucket ProjectBucket { get; private set; }
    int numRows = 1000;
    int numCols = 40;
    private string streamname;
    public float[,] XRdataArray { get; private set; }
    int rowIndex = 0;

    private string header = string.Empty;

    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {
        ProjectBucket = null;
        user_session = string.Empty;
    }

    void Awake()
    {
        streamname = Environment.GetEnvironmentVariable("KinesisFirehoseStreamName");
        //await SetSet3DestinationAsync();
        
        XRdataArray = new float[numRows, numCols];
        Debug.Log("Username is: " + userName);
        DateTime year_start = new DateTime(2023, 1, 1);
        TimeSpan total_minutes = DateTime.Now - year_start; 
        user_session = string.Format("{0}_{1}",userName,(int)total_minutes.TotalMinutes);
        string outputfile = string.Format("{0}viewinghistory.txt", user_session);
        writeXRdata = new StreamWriter(Path.Combine(Application.dataPath,outputfile), append: false);
        
        header = "";

        header += string.Format("Time\tHeadset_on\t");        

        string[] prefixes = new string[5] { "CE_", "LE_", "RE_", "LH_", "RH_" };
        string[] suffixes = new string[2] { "Pos", "Rot" };

        for (int i=0; i<Nodes.Length; i++)
        {
            Devices[i] = InputDevices.GetDeviceAtXRNode(Nodes[i]);
            string prefix = prefixes[i];
            for(int j=0; j<suffixes.Length; j++)
            {
                string suffix = suffixes[j];
                header += string.Format("{0}\t",prefix + suffix);
            }
        }

        prefixes = new string[1] { "Menu_" };
        suffixes = new string[5] { "Exists", "Bounds", "SM_Bounds", "Page_Viewed", "Panel_Viewed"};

        for(int i=0; i<suffixes.Length; i++)
        {
            header += string.Format("{0}\t", prefixes[0] + suffixes[i]);
        }

        prefixes = new string[1] { "Model_"};
        suffixes = new string[3] { "Name", "Centre", "Size" };
                
        for(int j =0; j<suffixes.Length;j++)
        {
            header += string.Format("{0}\t", prefixes[0] + suffixes[j]);
        }       

        prefixes = new string[1] { "Preview_"};
        suffixes = new string[2] { "Name", "Corners" };

        for (int i = 0; i < suffixes.Length; i++)
        {
            header += string.Format("{0}\t", prefixes[0] + suffixes[i]);
        }

        prefixes = new string[1] { "OtherComponentsMenu_" };
        suffixes = new string[2] { "Exists", "Corners" };

        for (int i = 0; i < suffixes.Length; i++)
        {
            header += string.Format("{0}\t", prefixes[0] + suffixes[i]);
        }

        MemoryStream header_stream = String_to_Stream(header);
        //await PutRecordAsync(header_stream);
        
        writeXRdata.WriteLine(header);
        //writeXRdata.WriteLine("CE_Pos\tCE_Rot\tCE_V\tCE_omega\tLE_Pos\tLE_Rot\tLE_V\tLE_omega\tRE_Pos\tRE_Rot\tRE_V\tRE_omega\tLH_Pos\tLH_Rot\tLH_V\tLH_omega\tRH_Pos\tRH_Rot\tRH_V\tRH_omega");
        writeXRdata.Flush();    
        
    }

    private void Start()
    {
        Get_Data();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Get_Data();
    } 

    private void Get_Data()
    {
        string the_Data = "";
        bool headset_tracked;        
        
        Devices[0].TryGetFeatureValue(CommonUsages.userPresence, out headset_tracked);
         
        the_Data += string.Format("{0}\t{1}\t", Time.realtimeSinceStartup, headset_tracked ? 1:0);

        for (int i = 0; i < Nodes.Length; i++)
        {
            Devices[i] = InputDevices.GetDeviceAtXRNode(Nodes[i]);
            Devices[i].TryGetFeatureValue(Positions[i], out Position_Values[i]);
            Devices[i].TryGetFeatureValue(Rotations[i], out Rotation_Values[i]);
            
            the_Data += string.Format("{0}\t{1}\t", Position_Values[i], Rotation_Values[i]);
        }

        int[] menu_array = new int[4] { 0, 0, 0, 0 };
        Vector3[] XRMenu_Bounds = new Vector3[4];
        Vector3[] SubMenu_Bounds = new Vector3[4];
        string submenu_view = "";
        string panel_view = "";
        if (LeftHandPresence.existingmenu)
        {
            menu_array[0] = 1;
            
            LeftHandPresence.existingmenu.BoundingRectangle.GetWorldCorners(XRMenu_Bounds);            

            if (LeftHandPresence.existingmenu.GetComponentInChildren<ChangeLesson>() != null) //change lesson was found
            {
                menu_array[1] = 1;
                LeftHandPresence.existingmenu.GetComponentInChildren<ChangeLesson>().BoundingRectangle.GetWorldCorners(SubMenu_Bounds);
                ChangeLesson m_submenu = LeftHandPresence.existingmenu.GetComponentInChildren<ChangeLesson>();
                Lesson m_lesson = m_submenu.Lessons[m_submenu.LessonIndex];
                submenu_view = m_lesson.GetComponentInChildren<TextMeshPro>().text;
                if(m_lesson.Panels[0].activeInHierarchy && !m_lesson.Panels[1].activeInHierarchy)
                {
                    panel_view = "summary";
                }
                else if(!m_lesson.Panels[0].activeInHierarchy && m_lesson.Panels[1].activeInHierarchy)
                {
                    panel_view = "discussions";
                }
            }

            else if (LeftHandPresence.existingmenu.GetComponentInChildren<ChangeLecturer>() != null) //change lecturer was found
            {
                menu_array[2] = 1;
                LeftHandPresence.existingmenu.GetComponentInChildren<ChangeLecturer>().BoundingRectangle.GetWorldCorners(SubMenu_Bounds);

            }

            else if (LeftHandPresence.existingmenu.GetComponentInChildren<UserSettings>() != null) //user is changing settings
            {
                menu_array[3] = 1;
                LeftHandPresence.existingmenu.GetComponentInChildren<UserSettings>().BoundingRectangle.GetWorldCorners(SubMenu_Bounds);
            }            
        }       

        string active_menus = string.Join(",", menu_array);

        string XRMenu_corners = string.Join(",", XRMenu_Bounds);
        string SubMenu_corners = string.Join(",", SubMenu_Bounds);

        the_Data += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t", active_menus, XRMenu_corners, SubMenu_corners, submenu_view,panel_view);

        string active_lesson_model = "";
        Vector3[] Lesson_model_bounds = new Vector3[2];

        string active_preview = "";
        string active_subtopic_card = "";

        Vector3[] Preview_Corners = new Vector3[4];
        Vector3[] SubTopicCard_Corners = new Vector3[4];
        if (LeftHandPresence.CurrentLesson && LeftHandPresence.CurrentLesson.isActiveAndEnabled)
        {
            active_lesson_model = LeftHandPresence.Chosen_Lesson_Model;
            Lesson_model_bounds[0] = LeftHandPresence.CurrentLesson.Lesson_Model_Bounds.center;
            Lesson_model_bounds[1] = LeftHandPresence.CurrentLesson.Lesson_Model_Bounds.size;

            if(LeftHandPresence.CurrentPreview && LeftHandPresence.CurrentPreview.activeInHierarchy)
            {
                active_preview  = LeftHandPresence.CurrentPreview.GetComponentInChildren<TextMeshPro>().text;                               
                LeftHandPresence.CurrentPreview.GetComponent<RectTransform>().GetWorldCorners(Preview_Corners);                
            }

            if(LeftHandPresence.CurrentSubTopicCard && LeftHandPresence.CurrentSubTopicCard.isActiveAndEnabled)
            {
                active_subtopic_card = LeftHandPresence.Chosen_subtopiccard;
                LeftHandPresence.CurrentSubTopicCard.GetComponent<RectTransform>().GetWorldCorners(SubTopicCard_Corners);
            }
        }
        
        string preview_corners = string.Join(",", Preview_Corners);
        string card_corners = string.Join(",", SubTopicCard_Corners);
        the_Data += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t", active_lesson_model, Lesson_model_bounds[0], Lesson_model_bounds[1], active_preview, preview_corners, active_subtopic_card, card_corners) ;

        if(LeftHandPresence.OtherComponentsAnchor != null && LeftHandPresence.OtherComponentsAnchor.gameObject.activeInHierarchy && LeftHandPresence.OtherComponentsAnchor.GetComponentInChildren<RectTransform>() != null)
        {
            RectTransform rect_transform = LeftHandPresence.OtherComponentsAnchor.GetComponentInChildren<RectTransform>();
            Vector3[] OtherMenu_Bounds = new Vector3[4];
            rect_transform.GetWorldCorners(OtherMenu_Bounds);
            string othermenu_corners = string.Join(",", OtherMenu_Bounds);
            the_Data += string.Format("{0}\t{1}", 1, othermenu_corners);
        }

        else
        {
            the_Data += string.Format("{0}\t{1}", 0, 0 );
        }

        writeXRdata.WriteLine(the_Data);
        writeXRdata.Flush();        
    }

    private async Task SetSet3DestinationAsync()
    {
        string roleARN = Environment.GetEnvironmentVariable("RoleARN");
        region = RegionEndpoint.USEast1;
        Debug.Log("roleARN is: " + roleARN);
        s3Client = new AmazonS3Client(region);
        ListBucketsResponse buckets = await s3Client.ListBucketsAsync();
        
        firhoseclient = new AmazonKinesisFirehoseClient(region);        
        Debug.Log("stream name is: " + streamname);

        DescribeDeliveryStreamRequest desc_stream_request = new DescribeDeliveryStreamRequest
        {
            DeliveryStreamName = streamname
        };

        DescribeDeliveryStreamResponse stream_description_resp = await firhoseclient.DescribeDeliveryStreamAsync(desc_stream_request);
        Debug.Log("DestinationID: " + stream_description_resp.DeliveryStreamDescription.Destinations[0].DestinationId);
        Debug.Log("StreamVersionID: " + stream_description_resp.DeliveryStreamDescription.VersionId);
        ProjectBucket = buckets.Buckets[0];
        string bucketARN = string.Format("arn:aws:s3:::{0}", ProjectBucket.BucketName);
        Debug.Log("Bucket ARN: " + bucketARN);
        ExtendedS3DestinationUpdate s3destionationconfig = new ExtendedS3DestinationUpdate
        {
            BucketARN = bucketARN,
            RoleARN = roleARN,
            Prefix = string.Format("SessionData/{0}/", MainMenu.login_session.username),
            /*
            BufferingHints = new BufferingHints
            {
                SizeInMBs = 128,
                IntervalInSeconds = 60,
            }
            */
        };            

        UpdateDestinationRequest updateDestinationRequest = new UpdateDestinationRequest
        {            
            DeliveryStreamName = streamname,            
            ExtendedS3DestinationUpdate = s3destionationconfig,
            CurrentDeliveryStreamVersionId = stream_description_resp.DeliveryStreamDescription.VersionId,
            DestinationId = stream_description_resp.DeliveryStreamDescription.Destinations[0].DestinationId
        };

        await firhoseclient.UpdateDestinationAsync(updateDestinationRequest);
        /*
        byte[] data = Encoding.UTF8.GetBytes(header); // the array data
        MemoryStream stream = new MemoryStream(data);

        Record XR_Record = new Record
        {
            Data = stream,
        };
        //string streamname = Environment.GetEnvironmentVariable("KinesisFirehoseStreamName");
        PutRecordRequest request = new PutRecordRequest
        {
            DeliveryStreamName = streamname,
            Record = XR_Record,
        };

        await firhoseclient.PutRecordAsync(request);
        Debug.Log("Put Record Sent");
        */
    }

    private MemoryStream String_to_Stream(string titles)
    {
        byte[] data = Encoding.UTF8.GetBytes(titles); // the array data
        MemoryStream stream = new MemoryStream(data);
        return stream;
    }

    private async Task PutRecordAsync(MemoryStream stream)
    {
        Record XR_Record = new Record
        {
            Data = stream,
        };
        //string streamname = Environment.GetEnvironmentVariable("KinesisFirehoseStreamName");
        PutRecordRequest request = new PutRecordRequest
        {
            DeliveryStreamName = streamname,
            Record = XR_Record,
            
        };
        await firhoseclient.PutRecordAsync(request);
        Debug.Log("Put Record Sent");
    }
    
}
