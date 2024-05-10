using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface IPlaylistService
{
    public Task<(Playlist?, string error)> GetPlaylist(Guid playlistGuid);
    
    public Task<(List<Playlist>?, string error)> GetPlaylists(Guid userGuid);
    
    public Task<(Guid? playlistGuid, string error)> CreatePlaylist(Playlist playlist);

    public Task<(bool successfully, string error)> EditPlaylist(Playlist playlist);
    
    public Task<(bool successfully, string error)> AddSampleToPlaylist(PlaylistSample playlistSample);

    public Task<(bool successfully, string error)> DeleteSampleFromPlaylist(Guid playlistSampleGuid);
    
    public Task<(bool successfully, string error)> DeletePlaylist(Guid playlistGuid);
}