using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface ISampleService
{
    public Task<(List<Sample>? samples, string error)> GetAll();

    public Task<(List<Sample>? samples, string error)> Search(string searchString);
    
    public Task<(Sample? sample, string error)> GetSample(Guid sampleGuid);

    public Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid);

    public Task<(bool successfully, string error)> AddAnListensToSample(Guid sampleGuid);
}