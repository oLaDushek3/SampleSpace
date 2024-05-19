using Amazon.S3;
using Microsoft.Extensions.Options;

namespace SampleSpaceDal.CloudStorage.Repositories;

public class BaseRepository
{
    private readonly CloudStorageOptions _options;
    
    protected string BucketName { get; }

    protected BaseRepository(IOptions<CloudStorageOptions> options)
    {
        _options = options.Value;
        BucketName = _options.BucketName;
    }
    
    protected AmazonS3Client GetClient()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = _options.ServiceUrl
        };
        
        return new AmazonS3Client(config);
    }
    
    protected string GetCreatedFileLink(string objectName) => $"https://{BucketName}.{_options.Hostname}/{objectName}";
}