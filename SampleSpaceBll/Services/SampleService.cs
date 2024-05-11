using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class SampleService(ISampleRepository sampleRepository) : ISampleService
{
    public async Task<(List<Sample>? samples, string error)> GetAll()
    {
        return await sampleRepository.GetAll();
    }
    
    public async Task<(List<Sample>? samples, string error)> Search(string searchString)
    {
        return await sampleRepository.Search(searchString);
    }

    public async Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid)
    {
        return await sampleRepository.GetByPlaylist(playlistGuid);
    }

    public async Task<(Sample? sample, string error)> GetSample(Guid sampleGuid)
    {
        return await sampleRepository.GetByGuid(sampleGuid);
    }
    
    public async Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid)
    {
        return await sampleRepository.GetUserSamples(userGuid);
    }
    
    public async Task<(bool successfully, string error)> AddAnListensToSample(Guid sampleGuid)
    {
        return await sampleRepository.AddAnListens(sampleGuid);
    }
}