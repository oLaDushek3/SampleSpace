using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleRepository
{
    public Task<List<Sample>> GetAll();

    public Task<List<Sample>> Search(string searchString);

    public Task<Sample?> GetByGuid(Guid sampleGuid);

    public Task<List<Sample>> GetUserSamples(Guid userGuid);

    public Task<bool> AddAnListens(Guid sampleGuid);
}