using SampleSpaceCore.Models.Sample;

namespace SampleSpaceCore.Abstractions.PostgreSQL.Repositories;

public interface ISampleRepository
{
    public Task<(List<Sample>? samples, string error)> GetAll();

    public Task<(List<Sample>? samples, string error)> GetSortByDate(int limit, int numberOfPage);
    
    public Task<(List<Sample>? samples, string error)> GetSortByListens(int limit, int numberOfPage);
    
    public Task<(List<Sample>? samples, string error)> Search(string searchString, int limit, int numberOfPage);
    
    public Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid, int limit, int numberOfPage);
    
    public Task<(Sample? sample, string error)> GetByGuid(Guid sampleGuid);

    public Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid, int limit, int numberOfPage);

    public Task<(bool successfully, string error)> AddAnListens(Guid sampleGuid);
    
    public Task<(Guid? sampleGuid, string error)> Create(Sample sample);
    
    public Task<(bool successfully, string error)> Delete(Guid sampleGuid);
}