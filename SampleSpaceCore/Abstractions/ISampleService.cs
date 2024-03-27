using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleService
{
    public Task<List<Sample>> GetAll();

    public Task<List<Sample>> Search(string searchString);

    public Task<List<Sample>> GetUserSamples(Guid userGuid);

    public Task<bool> AddAnListensToSample(Guid sampleGuid);
}