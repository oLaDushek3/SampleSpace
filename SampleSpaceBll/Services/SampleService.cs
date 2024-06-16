using SampleSpaceBll.Abstractions.Sample;
using SampleSpaceCore.Abstractions.Services;
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

    public async Task<(List<Sample>? samples, string error)> GetSortByDate(int limit, int numberOfPage)
    {
        return await postgreSqlSampleRepository.GetSortByDate(limit, numberOfPage);
    }

    public async Task<(List<Sample>? samples, string error)> GetSortByListens(int limit, int numberOfPage)
    {
        return await postgreSqlSampleRepository.GetSortByListens(limit, numberOfPage);
    }

    public async Task<(List<Sample>? samples, string error)> Search(string searchString, int limit, int numberOfPage)
    {
        return await postgreSqlSampleRepository.Search(searchString, limit, numberOfPage);
    }

    public async Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid, int limit,
        int numberOfPage)
    {
        return await postgreSqlSampleRepository.GetByPlaylist(playlistGuid, limit, numberOfPage);
    }

    public async Task<(Sample? sample, string error)> GetSample(Guid sampleGuid)
    {
        return await postgreSqlSampleRepository.GetByGuid(sampleGuid);
    }

    public async Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid, int limit, int numberOfPage)
    {
        return await postgreSqlSampleRepository.GetUserSamples(userGuid, limit, numberOfPage);
    }

    public async Task<(bool successfully, string error)> AddAnListensToSample(Guid sampleGuid)
    {
        return await postgreSqlSampleRepository.AddAnListens(sampleGuid);
    }

    public (Stream? trimmedSampleStream, string error) TrimSample(Stream sampleFileStream, double sampleStart,
        double sampleEnd, string sampleExtension)
    {
        var (sampleStream, error) = sampleTrimmer.TestTrimMp3File(sampleFileStream, TimeSpan.FromSeconds(sampleStart),
            TimeSpan.FromSeconds(sampleEnd), sampleExtension);

        return !string.IsNullOrEmpty(error) ? (null, error) : (sampleStream, string.Empty);
    }

    public async Task<(string? sampleLink, string? coverLink, string error)> UploadSample(Guid userGuid,
        string sampleName, Stream sampleStream, string sampleFileExtension, Stream coverStream,
        string coverFileExtension)
    {
        var (sampleLink, coverLink, error) =
            await cloudStorageSampleRepository.Create(userGuid, sampleName, sampleStream, sampleFileExtension,
                coverStream, coverFileExtension);

        return !string.IsNullOrEmpty(error) ? (null, null, error) : (sampleLink, coverLink, string.Empty);
    }

    public async Task<(Guid? sampleGuid, string error)> CreateSample(Sample sample)
    {
        return await postgreSqlSampleRepository.Create(sample);
    }

    public async Task<(bool successfully, string error)> DeleteSample(Sample sample)
    {
        var deleteFromPostgres = await postgreSqlSampleRepository.Delete(sample.SampleGuid);

        if (!string.IsNullOrEmpty(deleteFromPostgres.error))
            return (false, deleteFromPostgres.error);

        return await cloudStorageSampleRepository.Delete(sample.UserGuid, sample.Name);
    }
}