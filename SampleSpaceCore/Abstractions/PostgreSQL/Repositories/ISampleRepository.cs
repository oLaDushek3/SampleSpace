using SampleSpaceCore.Models;
using SampleSpaceCore.Models.Sample;

namespace SampleSpaceCore.Abstractions.PostgreSQL.Repositories;

public interface ISampleRepository
{
    public Task<(List<Sample>? samples, string error)> GetAll();

    public Task<(List<Sample>? samples, string error)> Search(string searchString);
    
    public Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid);
    
    public Task<(Sample? sample, string error)> GetByGuid(Guid sampleGuid);

    public Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid);

    public Task<(bool successfully, string error)> AddAnListens(Guid sampleGuid);
    
    public Task<(Guid? sampleGuid, string error)> Create(Sample sample);
    
    public Task<(bool successfully, string error)> Delete(Guid sampleGuid);
}