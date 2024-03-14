using Amazon.S3;
using Amazon.S3.Model;

namespace SampleSpaceInfrastructure;

public class s3test
{
    public async Task<string> Main()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = "https://hb.vkcs.cloud",
        };
        
        var client = new AmazonS3Client(config);
        
        var request = new PutObjectRequest
        {
            BucketName = "sample_space",
            Key = "posts/test-sample/my-test-sample.mp3",
            FilePath = "C://Users//oLaDushek//Desktop//sample.mp3",
            CannedACL = S3CannedACL.PublicRead
        };
        
        var response = await client.PutObjectAsync(request);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            return "https://sample_space.hb.ru-msk.vkcs.cloud/my-test-sample";
        }
        
        return string.Empty;
    }
}