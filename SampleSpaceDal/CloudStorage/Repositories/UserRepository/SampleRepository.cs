using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using SampleSpaceCore.Abstractions.CloudStorage.Repositories;

namespace SampleSpaceDal.CloudStorage.Repositories.UserRepository;

public class UserRepository(IOptions<CloudStorageOptions> options) : BaseRepository(options), IUserRepository
{
    public async Task<(string? avatarLink, string error)> Create(Guid userGuid, Stream avatarStream)
    {
        var client = GetClient();
        
        var sampleObjectName = $"avatars/{userGuid}.jpg";
        var request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = sampleObjectName,
            InputStream = avatarStream,
            CannedACL = S3CannedACL.PublicRead
        };
        
        try
        {
            await client.PutObjectAsync(request);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }

        return (GetCreatedFileLink(sampleObjectName), string.Empty);
    }

    public async Task<(bool successfully, string error)> Delete(Guid userGuid)
    {
        var objectName = $"avatars/{userGuid}.jpg";

        var request = new DeleteObjectRequest()
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