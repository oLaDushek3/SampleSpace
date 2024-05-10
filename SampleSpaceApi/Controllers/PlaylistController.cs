using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.Playlist;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/playlist")]
public class PlaylistController(IPlaylistService playlistService) : ControllerBase
{
    [HttpGet("get-user-playlists")]
    public async Task<IActionResult> GetSampleComments([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var (playlists, error) = await playlistService.GetPlaylists(userGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (playlists == null)
            return NotFound();
        
        return Ok(playlists); 
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist(CreatePlaylistRequest request)
    {
        //Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != request.UserGuid)
        //     return Forbid();
        
        var (requestPlaylist, requestError) =
            Playlist.Create(Guid.NewGuid(), request.UserGuid, request.Name);
        
        if (!string.IsNullOrEmpty(requestError))
            return BadRequest(requestError);
            
        var (playlists, error) = await playlistService.CreatePlaylist(requestPlaylist!);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (playlists == null)
            return NotFound();
        
        return Ok(playlists); 
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPut("edit-playlist")]
    public async Task<IActionResult> EditPlaylist(EditPlaylistRequest request)
    {
        var (playlist, getError) = await playlistService.GetPlaylist(request.PlaylistGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != comment!.UserGuid)
        //     return Forbid();

        var (modifiedPlaylist, modifiedError) = playlist!.Edit(request.Name);
        
        if(!string.IsNullOrEmpty(modifiedError))
            return  BadRequest(modifiedError);
        
        var (successfully, editError) = await playlistService.EditPlaylist(modifiedPlaylist!);
        
        if(!string.IsNullOrEmpty(editError))
            return  BadRequest(editError);

        return successfully ? Ok() : BadRequest("Server error");
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPost("add-sample-to-playlist")]
    public async Task<IActionResult> AddSampleToPlaylist(AddSampleToPlaylistRequest request)
    {
        // var (playlist, getError) = await playlistService.GetPlaylist(request.PlaylistGuid);
        //
        // if(!string.IsNullOrEmpty(getError))
        //     return  BadRequest(getError);

        // // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != playlist!.UserGuid)
        //     return Forbid();
        
        var (requestPlaylistSample, requestError) =
            PlaylistSample.Create(Guid.NewGuid(), request.PlaylistGuid, request.SampleGuid);
        
        if (!string.IsNullOrEmpty(requestError))
            return BadRequest(requestError);
            
        var (playlists, error) = await playlistService.AddSampleToPlaylist(requestPlaylistSample!);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (!playlists)
            return NotFound();
        
        return Ok(); 
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpDelete("delete-sample-from-playlist")]
    public async Task<IActionResult> DeleteSampleFromPlaylist([FromQuery(Name = "playlist-sample-guid")] Guid playlistSampleGuid)
    {
        var (playlist, getError) = await playlistService.GetPlaylist(playlistSampleGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != playlist!.UserGuid)
        //     return Forbid();
        
        var (successfully, deleteError) = await playlistService.DeleteSampleFromPlaylist(playlistSampleGuid);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
    
    //Доработать проверку права доступа
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpDelete("delete-playlist")]
    public async Task<IActionResult> DeletePlaylist([FromQuery(Name = "playlist-guid")] Guid playlistGuid)
    {
        var (playlist, getError) = await playlistService.GetPlaylist(playlistGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != comment!.UserGuid)
        //     return Forbid();

        var (successfully, deleteError) = await playlistService.DeletePlaylist(playlistGuid);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
}