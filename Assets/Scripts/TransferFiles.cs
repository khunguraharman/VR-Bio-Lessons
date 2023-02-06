using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using System.Threading.Tasks;
using Amazon.Runtime;

public class TransferFiles : MonoBehaviour
{
    public static bool Update_Lessons = false; // will update under changelesson
    public static bool Perform_UpdateCheck = true;

    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
    public static string ProjectBucketName { get; private set; }
    public static S3Bucket ProjectBucket { get; private set; }
    public static List<string> S3_lessonsummary_json  { get; private set; }

    public AmazonS3Client s3Client;
    // Start is called before the first frame update
    private void Awake()
    {
        S3_lessonsummary_json = new List<string>();
        s3Client = new AmazonS3Client(bucketRegion);
        
        List_Buckets();
    }
    public async void List_Buckets()
    {
        ListBucketsResponse buckets = await s3Client.ListBucketsAsync();        
        ProjectBucket = buckets.Buckets[0];
        ProjectBucketName = ProjectBucket.BucketName;
        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = ProjectBucketName,            
            Prefix = LeftHandPresence.build_info.ToString() + "_lessonsummary_"
        };

        ListObjectsV2Response response = await s3Client.ListObjectsV2Async(request); 

        for(int i=0; i<response.S3Objects.Count; i++) //go through each 
        {
            S3_lessonsummary_json.Add(response.S3Objects[i].Key);
        }

        request = new ListObjectsV2Request
        {
            BucketName = ProjectBucketName,
            Prefix = LeftHandPresence.build_info.ToString() + "_lessonsummary_"
        };



    }
    
}

    
    


