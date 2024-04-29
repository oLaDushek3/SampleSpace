using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class SampleService(ISampleRepository sampleRepository) : ISampleService
{
    public async Task<List<Sample>> GetAll()
    {
        return await sampleRepository.GetAll();
    }
    
    public async Task<List<Sample>> Search(string searchString)
    {
        return await sampleRepository.Search(searchString);
    }

    public async Task<Sample?> GetSample(Guid sampleGuid)
    {
        return await sampleRepository.GetByGuid(sampleGuid);
    }
    
    public async Task<List<Sample>> GetUserSamples(Guid userGuid)
    {
        return await sampleRepository.GetUserSamples(userGuid);
    }
    
    public async Task<bool> AddAnListensToSample(Guid sampleGuid)
    {
        return await sampleRepository.AddAnListens(sampleGuid);
    }
}