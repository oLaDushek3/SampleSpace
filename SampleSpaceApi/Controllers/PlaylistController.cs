using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetUserPlaylists([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var (playlists, error) = await playlistService.GetUserPlaylists(userGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (playlists == null)
            return NotFound();
        
        return Ok(playlists); 
    }
    
    [HttpGet("get-user-playlists-relative-sample")]
    public async Task<IActionResult> GetUserPlaylistsRelativeSample([FromQuery(Name = "user-guid")] Guid userGuid, [FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (playlists, error) = await playlistService.GetUserPlaylistsRelativeSample(userGuid, sampleGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (playlists == null)
            return NotFound();
        
        return Ok(playlists); 
    }
    
    // Раскомментировать после развертывания на сервере
    [Authorize]
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist(CreatePlaylistRequest request)
    {
        //Раскомментировать после развертывания на сервере
        var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        
        if (new Guid(loginUserGuid) != request.UserGuid)
            return Forbid();
        
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
        
        var modified= playlist!.Edit(request.Name);
        
        if(!string.IsNullOrEmpty(modified.error))
            return  BadRequest(modified.error);
        
        var (successfully, editError) = await playlistService.EditPlaylist(playlist);
        
        if(!string.IsNullOrEmpty(editError))
            return  BadRequest(editError);

        return successfully ? Ok() : BadRequest("Server error");
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPost("add-sample-to-playlist")]
    public async Task<IActionResult> AddSampleToPlaylist(AddSampleToPlaylistRequest request)
    {
        var (playlist, getError) = await playlistService.GetPlaylist(request.PlaylistGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != playlist!.UserGuid)
        //     return Forbid();

        var (alreadyContain, alreadyContainError) =
            await playlistService.CheckSampleContain(request.PlaylistGuid, request.SampleGuid);
        
        if (!string.IsNullOrEmpty(alreadyContainError))
            return BadRequest(alreadyContainError);

        if (alreadyContain)
            return BadRequest("Already contain");

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
    public async Task<IActionResult> DeleteSampleFromPlaylist([FromQuery(Name = "playlist-guid")] Guid playlistGuid, [FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (playlist, getError) = await playlistService.GetPlaylist(playlistGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != playlist!.UserGuid)
        //     return Forbid();
        
        var (successfully, deleteError) = await playlistService.DeleteSampleFromPlaylist(playlistGuid, sampleGuid);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
    
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
        // if (new Guid(loginUserGuid) != playlist!.UserGuid)
        //     return Forbid();
        
        var (successfully, deleteError) = await playlistService.DeletePlaylist(playlist!);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
}