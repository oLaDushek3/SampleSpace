using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.PostgreSQL.Repositories;

public interface IPlaylistRepository
{
    public Task<(Playlist?, string error)> GetByGuid(Guid playlistGuid);
    
    public Task<(List<Playlist>?, string error)> Get(Guid userGuid);
    
    public Task<(Guid? playlistGuid, string error)> Create(Playlist playlist);
    
    public Task<(bool successfully, string error)> Edit(Playlist playlist);

    public Task<(bool successfully, string error)> AddSample(PlaylistSample playlistSample);
    
    public Task<(bool successfully, string error)> CheckSampleContain(Guid playlistGuid, Guid sampleGuid);
    
    public Task<(bool successfully, string error)> DeleteSample(Guid playlistGuid, Guid sampleGuid);
    
    public Task<(bool successfully, string error)> Delete(Guid playlistGuid);
}