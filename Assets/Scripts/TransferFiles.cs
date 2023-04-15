using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using System.Threading.Tasks;
using Amazon.Runtime;
using TMPro;
using System.Linq;

public class TransferFiles : MonoBehaviour
{
    public ChangeLesson ChangeLessonMenu { get; private set; }
    public Lesson[] Lessons { get; private set; }
    public LessonModel[] Models { get; private set; }
    public SubTopic[] SubTopics { get; private set; }
    public string[] Lesson_meta_data_collected { get; private set; }

    public static bool Update_Lessons = false; // will update under changelesson
    public static bool Perform_UpdateCheck = true;

    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
    public static string ProjectBucketName { get; private set; }
    public static S3Bucket ProjectBucket { get; private set; }
    public string[] S3_lessonsummary_json { get; private set; }
    public string[] S3_subtopiccontents_json { get; private set; }

    private AmazonS3Client s3Client;
    private TransferUtility transferUtility;
    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {        
        Update_Lessons = false;
        Perform_UpdateCheck = true;
        ProjectBucketName = string.Empty;
        ProjectBucket = null;
    }
    private void Awake()
    {
        s3Client = new AmazonS3Client(bucketRegion);
        transferUtility = new TransferUtility(s3Client);
        GetS3MetaData();
    }

    public async void GetS3MetaData()
    {
        ListBucketsResponse buckets = await s3Client.ListBucketsAsync();
        ProjectBucket = buckets.Buckets[0];
        ProjectBucketName = ProjectBucket.BucketName;

        //get all the lesson summary meta data already in S3
        ListObjectsV2Request request_lessonsummaries = new ListObjectsV2Request
        {
            BucketName = ProjectBucketName,
            Prefix = LeftHandPresence.build_info.ToString() + "_lessonsummary_"
        };
        ListObjectsV2Response response = await s3Client.ListObjectsV2Async(request_lessonsummaries);
        S3_lessonsummary_json = new string[response.S3Objects.Count];
        for (int i = 0; i < response.S3Objects.Count; i++) //go through each 
        {
            S3_lessonsummary_json[i] = response.S3Objects[i].Key;
        }

        //get all the subtopic meta data already in S3
        ListObjectsV2Request request_subtopiccontents = new ListObjectsV2Request
        {
            BucketName = ProjectBucketName,
            Prefix = LeftHandPresence.build_info.ToString() + "_subtopiccontents_"
        };
        response = await s3Client.ListObjectsV2Async(request_subtopiccontents);
        S3_subtopiccontents_json = new string[response.S3Objects.Count];
        for (int i = 0; i < response.S3Objects.Count; i++) //go through each 
        {
            S3_subtopiccontents_json[i] = response.S3Objects[i].Key;
        }

        //compare the 
        ChangeLessonMenu = Resources.Load<ChangeLesson>("Menus/ChangeLessonSubMenu");
        if (ChangeLessonMenu != null)
        {
            Debug.Log("Was able to load the lesson menu data");
        }
        else
        {
            Debug.Log("Was not able to load the or find the lesson menu");
        }
        int total_lessons_in_current_build = ChangeLessonMenu.Lessons.Length;
        Lesson_meta_data_collected = new string[total_lessons_in_current_build];

        Lessons = new Lesson[total_lessons_in_current_build];
        Lessons = ChangeLessonMenu.Lessons;
        Models = new LessonModel[total_lessons_in_current_build];
        Models = ChangeLessonMenu.LessonModels;

        //Check lessons
        for (int i = 0; i < total_lessons_in_current_build; i++)
        {
            TextMeshPro lesson_tmp = Lessons[i].Get_TMPro_Object();
            string lesson_m = lesson_tmp.text; //all the lessons in the current build                                                                                                                   
            string search_for = LeftHandPresence.build_info + "_lessonsummary_" + lesson_m + ".json";// e.g 0_0_1_lessonsummary_Animal Cell.json
            if (S3_lessonsummary_json.Any(name => name == search_for)) //if the latest lessonsummary is in the S3 list, just move onto the next one
            {
                Debug.Log(string.Format("{0} already exists in the S3 bucket", search_for));
                continue;
            }
            else //log a new JSON file and upload to the bucket
            {
                Lessons[i].Log_LessonJSON(lesson_tmp);
                string filepath = string.Format("{0}/{1}", Application.dataPath, search_for);
                await transferUtility.UploadAsync(filepath, ProjectBucketName, search_for);
                Debug.Log(string.Format("{0} was uploaded to the S3 bucket", search_for));
            }
        }

        //Check models
        for (int i = 0; i < total_lessons_in_current_build; i++)
        {
            TextMeshPro lesson_tmp = Lessons[i].Get_TMPro_Object();
            string lesson_m = lesson_tmp.text; //all the lessons in the current build 
            SubTopic[] subtopics = Models[i].Components;
            for (int j = 0; j < subtopics.Length; j++)
            {
                string subtopic_name = subtopics[j].SubTopicTMP().text;
                string search_for = LeftHandPresence.build_info + "_subtopiccontents_" + subtopic_name + "_" + lesson_m + ".json";// e.g 0_0_1_subtopiccontents_Nucleus_Animal Cell.json
                if (S3_subtopiccontents_json.Any(name => name == search_for)) //if the latest lessonsummary is in the S3 list, just move onto the next one
                {
                    Debug.Log(string.Format("{0} already exists in the S3 bucket", search_for));
                    continue;
                }
                else //log a new JSON file and upload to the bucket
                {
                    subtopics[j].LogSubTopicData(lesson_m);
                    string filepath = string.Format("{0}/{1}", Application.dataPath, search_for);
                    Debug.Log("trying to upload the file" + search_for);
                    await transferUtility.UploadAsync(filepath, ProjectBucketName, search_for);
                    Debug.Log(string.Format("{0} was uploaded to the S3 bucket", search_for));
                }
            }
        }
    }
}
   

    


    
    


