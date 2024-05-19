using SampleSpaceBll.Abstractions.Sample;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;
using SampleSpaceCore.Models.Sample;
using IPostgreSQLSampleRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.ISampleRepository;
using ICloudStorageSampleRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.ISampleRepository;

namespace SampleSpaceBll.Services;

public class SampleService(IPostgreSQLSampleRepository postgreSqlSampleRepository, ISampleTrimmer sampleTrimmer,
    ICloudStorageSampleRepository cloudStorageSampleRepository) : ISampleService
{
    public async Task<(List<Sample>? samples, string error)> GetAll()
    {
        return await postgreSqlSampleRepository.GetAll();
    }

    public async Task<(List<Sample>? samples, string error)> Search(string searchString)
    {
        return await postgreSqlSampleRepository.Search(searchString);
    }

    public async Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid)
    {
        return await postgreSqlSampleRepository.GetByPlaylist(playlistGuid);
    }

    public async Task<(Sample? sample, string error)> GetSample(Guid sampleGuid)
    {
        return await postgreSqlSampleRepository.GetByGuid(sampleGuid);
    }

    public async Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid)
    {
        return await postgreSqlSampleRepository.GetUserSamples(userGuid);
    }

    public async Task<(bool successfully, string error)> AddAnListensToSample(Guid sampleGuid)
    {
        return await postgreSqlSampleRepository.AddAnListens(sampleGuid);
    }

    public (Stream? trimmedSampleStream, string error) TrimSample(Stream sampleFileStream, double sampleStart, double sampleEnd)
    {
        var (sampleStream, error) = sampleTrimmer.TrimMp3File(sampleFileStream,
            TimeSpan.FromSeconds(sampleStart),
            TimeSpan.FromSeconds(sampleEnd));

        return !string.IsNullOrEmpty(error) ? (null, error) : (sampleStream, string.Empty);
    }

    public async Task<(string? sampleLink, string? coverLink, string error)> UploadSample(Guid userGuid,
        string sampleName, Stream sampleStream, Stream coverStream)
    {
        var (sampleLink, coverLink, error) =
            await cloudStorageSampleRepository.Create(userGuid, sampleName, sampleStream, coverStream);

        return !string.IsNullOrEmpty(error) ? (null, null, error) : (sampleLink, coverLink, string.Empty);
    }

    public async Task<(Guid? sampleGuid, string error)> CreateSample(Sample sample)
    {
        return await postgreSqlSampleRepository.Create(sample);
    }

    public async Task<(bool successfully, string error)> DeleteSample(Guid sampleGuid)
    {
        return await postgreSqlSampleRepository.Delete(sampleGuid);
    }
}