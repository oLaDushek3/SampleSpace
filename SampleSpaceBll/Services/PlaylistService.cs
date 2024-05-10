using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class PlaylistService(IPlaylistRepository playlistRepository, ISampleRepository sampleRepository) : IPlaylistService
{
    public async Task<(Playlist?, string error)> GetPlaylist(Guid playlistGuid)
    {
        return await playlistRepository.GetByGuid(playlistGuid);
    }

    public async Task<(List<Playlist>?, string error)> GetPlaylists(Guid userGuid)
    {
        return await playlistRepository.Get(userGuid);
    }

    public async Task<(Guid? playlistGuid, string error)> CreatePlaylist(Playlist playlist)
    {
        return await playlistRepository.Create(playlist);
    }

    public async Task<(bool successfully, string error)> EditPlaylist(Playlist playlist)
    {
        return await playlistRepository.Edit(playlist);
    }

    public async Task<(bool successfully, string error)> AddSampleToPlaylist(PlaylistSample playlistSample)
    {
        var (_, sampleError) = await sampleRepository.GetByGuid(playlistSample.SampleGuid);

        if (!string.IsNullOrEmpty(sampleError))
            return (false, sampleError);
        
        var (_, playlistError) = await playlistRepository.GetByGuid(playlistSample.PlaylistGuid);

        if (!string.IsNullOrEmpty(playlistError))
            return (false, playlistError);
        
        return await playlistRepository.AddSample(playlistSample);
    }

    public async Task<(bool successfully, string error)> DeleteSampleFromPlaylist(Guid playlistSampleGuid)
    {
        return await playlistRepository.DeleteSample(playlistSampleGuid);
    }

    public async Task<(bool successfully, string error)> DeletePlaylist(Guid playlistGuid)
    {
        return await playlistRepository.Delete(playlistGuid);
    }
}