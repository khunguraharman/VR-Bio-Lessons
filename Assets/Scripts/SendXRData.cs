using Amazon.KinesisFirehose.Model;
using Amazon.KinesisFirehose;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using UnityEngine;

public class SendXRData : MonoBehaviour
{
    private string stringdata = "";
    static string userName = System.Environment.UserName;
    public AmazonKinesisFirehoseClient firhoseclient { get; private set; }
    public RegionEndpoint region { get; private set; }
    private AmazonS3Client s3Client;
    public static S3Bucket ProjectBucket { get; private set; }
    private string streamname;

    
    /*
    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {
        ProjectBucket = null;        
    }
    */

    [SerializeField] private AllXRData allxrdata;
    async void Awake()
    {
        streamname = Environment.GetEnvironmentVariable("KinesisFirehoseStreamName");        
        await SetSet3DestinationAsync();
        allxrdata.ConfigureUpload(PutRecordAsync);
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
            //Prefix = string.Format("SessionData/{0}/", MainMenu.login_session.username),
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
        Debug.Log("The Destinate Has Been Configured");
    }    

    private async Task PutRecordAsync(string stringdata)
    {
        Debug.Log("Began Asynchronous Upload");     
        byte[] data = Encoding.UTF8.GetBytes(stringdata);
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
    }
}
