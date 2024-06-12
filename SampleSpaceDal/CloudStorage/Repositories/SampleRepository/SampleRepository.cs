using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using SampleSpaceCore.Abstractions.CloudStorage.Repositories;

namespace SampleSpaceDal.CloudStorage.Repositories.SampleRepository;

public class SampleRepository(IOptions<CloudStorageOptions> options) : BaseRepository(options), ISampleRepository
{
    public async Task<(string? sampleLink, string? coverLink, string error)> Create(Guid userGuid, string sampleName,
        Stream sampleStream, Stream coverStream)
    {
        var client = GetClient();
        
        var sampleObjectName = $"samples/{userGuid}/{sampleName}/{sampleName}.mp3";
        var request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = sampleObjectName,
            InputStream = sampleStream,
            CannedACL = S3CannedACL.PublicRead
        };
        
        try
        {
            await client.PutObjectAsync(request);
        }
        catch (Exception exception)
        {
            return (null, null, exception.Message);
        }
        
        var coverObjectName = $"samples/{userGuid}/{sampleName}/{sampleName}.jpg";
        request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = coverObjectName,
            InputStream = coverStream,
            CannedACL = S3CannedACL.PublicRead
        };

        client = GetClient();
        
        try
        {
            await client.PutObjectAsync(request);
        }
        catch (Exception exception)
        {
            return (null, null, exception.Message);
        }

        return (GetCreatedFileLink(sampleObjectName), GetCreatedFileLink(coverObjectName), string.Empty);
    }

    public async Task<(string? coverLink, string error)> CreateCover(Guid userGuid, string sampleName,
        MemoryStream coverStream)
    {
        var objectName = $"samples/{userGuid}/{sampleName}/{sampleName}.png";

        var request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = objectName,
            InputStream = coverStream,
            CannedACL = S3CannedACL.PublicRead
        };

        var client = GetClient();
        try
        {
            await client.PutObjectAsync(request);
            return (GetCreatedFileLink(objectName), string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
    }

    public async Task<(bool successfully, string error)> Delete(Guid userGuid, string sampleName)
    {
        var objectName = $"posts/{userGuid}/{sampleName}";

        var request = new DeleteObjectRequest
        {
            BucketName = BucketName,
            Key = objectName,
        };

        var client = GetClient();
        try
        {
            await client.DeleteObjectAsync(request);
            return (true, string.Empty);
        }
        catch (Exception exception)
        {
            return (false, exception.Message);
        }
    }
}