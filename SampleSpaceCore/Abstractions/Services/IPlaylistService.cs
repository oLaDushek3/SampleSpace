using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface IPlaylistService
{
    public Task<(Playlist? playlist, string error)> GetPlaylist(Guid playlistGuid);
    
    public Task<(List<Playlist>? playlists, string error)> GetUserPlaylists(Guid userGuid);

    public Task<(bool successfully, string error)> CheckSampleContain(Guid playlistGuid, Guid sampleGuid);
    
    public Task<(List<PlaylistRelativeSample>?, string error)> GetUserPlaylistsRelativeSample(Guid userGuid, Guid sampleGuid);
    
    public Task<(Guid? playlistGuid, string error)> CreatePlaylist(Playlist playlist);
    
    public Task<(bool successfully, string error)> EditPlaylist(Playlist playlist);
    
    public Task<(bool successfully, string error)> AddSampleToPlaylist(PlaylistSample playlistSample);

    public Task<(bool successfully, string error)> DeleteSampleFromPlaylist(Guid playlistSampleGuid);
    
    public Task<(bool successfully, string error)> DeletePlaylist(Guid playlistGuid);
}