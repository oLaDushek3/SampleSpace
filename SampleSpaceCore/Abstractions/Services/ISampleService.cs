using SampleSpaceCore.Models;
using SampleSpaceCore.Models.Sample;

namespace SampleSpaceCore.Abstractions.Services;

public interface ISampleService
{
    public Task<(List<Sample>? samples, string error)> GetAll();

    public Task<(List<Sample>? samples, string error)> Search(string searchString);

    public Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid);

    public Task<(Sample? sample, string error)> GetSample(Guid sampleGuid);

    public Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid);

    public Task<(bool successfully, string error)> AddAnListensToSample(Guid sampleGuid);

    public (Stream? trimmedSampleStream, string error) TrimSample(Stream sampleFileStream, double sampleStart, double sampleEnd);
    
    public Task<(string? sampleLink, string? coverLink, string error)> UploadSample(Guid userGuid,
        string sampleName, Stream sampleStream, Stream coverStream);

    public Task<(Guid? sampleGuid, string error)> CreateSample(Sample sample);

    public Task<(bool successfully, string error)> DeleteSample(Sample sample);
}