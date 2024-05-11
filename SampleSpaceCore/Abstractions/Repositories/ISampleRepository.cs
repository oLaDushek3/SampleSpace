using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Repositories;

public interface ISampleRepository
{
    public Task<(List<Sample>? samples, string error)> GetAll();

    public Task<(List<Sample>? samples, string error)> Search(string searchString);
    
    public Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid);
    
    public Task<(Sample? sample, string error)> GetByGuid(Guid sampleGuid);

    public Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid);

    public Task<(bool successfully, string error)> AddAnListens(Guid sampleGuid);
}