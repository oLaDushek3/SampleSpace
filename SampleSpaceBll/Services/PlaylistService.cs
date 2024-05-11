using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class PlaylistService(IPlaylistRepository playlistRepository, ISampleRepository sampleRepository) : IPlaylistService
{
    public async Task<(Playlist? playlist, string error)> GetPlaylist(Guid playlistGuid)
    {
        return await playlistRepository.GetByGuid(playlistGuid);
    }

    public async Task<(List<Playlist>? playlists, string error)> GetUserPlaylists(Guid userGuid)
    {
        return await playlistRepository.Get(userGuid);
    }

    public async Task<(bool successfully, string error)> CheckSampleContain(Guid playlistGuid, Guid sampleGuid)
    {
        return await playlistRepository.CheckSampleContain(playlistGuid, sampleGuid);
    }

    public async Task<(List<PlaylistRelativeSample>?, string error)> GetUserPlaylistsRelativeSample(Guid userGuid, Guid sampleGuid)
    {
        var (playlists, playlistsError) = await GetUserPlaylists(userGuid);

        if (!string.IsNullOrEmpty(playlistsError))
            return (null, playlistsError);

        var playlistsRelativeSample = new List<PlaylistRelativeSample>();
        
        foreach (var playlist in playlists!)
        {
            var (contain, existError) = await CheckSampleContain(playlist.PlaylistGuid, sampleGuid);
            
            if (!string.IsNullOrEmpty(existError))
                return (null, existError);

            var (playlistRelativeSample, playlistRelativeSampleError) = PlaylistRelativeSample.Create(playlist, contain);
            
            if (!string.IsNullOrEmpty(playlistRelativeSampleError))
                return (null, playlistRelativeSampleError);
            
            playlistsRelativeSample.Add(playlistRelativeSample!);
        }

        return (playlistsRelativeSample, string.Empty);
    }

    public async Task<(Guid? playlistGuid, string error)> CreatePlaylist(Playlist playlist)
    {
        return await playlistRepository.Create(playlist);
    }

    public async Task<(bool successfully, string error)> EditPlaylist(Playlist playlist)
    {
        if (!playlist!.CanBeModified)
            return (false, "Playlist cannot be modified");
        
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

    public async Task<(bool successfully, string error)> DeleteSampleFromPlaylist(Guid playlistGuid, Guid sampleGuid)
    {
        return await playlistRepository.DeleteSample(playlistGuid, sampleGuid);
    }

    public async Task<(bool successfully, string error)> DeletePlaylist(Playlist playlist)
    {
        if (!playlist!.CanBeModified)
            return (false, "Playlist cannot be deleted");
        
        return await playlistRepository.Delete(playlist.PlaylistGuid);
    }
}