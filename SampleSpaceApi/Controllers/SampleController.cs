using Microsoft.AspNetCore.Mvc;
using SampleSpaceCore.Abstractions.Services;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/sample")]
public class SampleController(ISampleService sampleService) : ControllerBase
{
    [HttpGet("get-all-samples")]
    public async Task<IActionResult> GetAllSamples()
    {
        var (samples, error) = await sampleService.GetAll();
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (samples == null || samples.Count == 0)
            return NotFound();
        
        return Ok(samples); 
    }

    [HttpGet("search-samples")]
    public async Task<IActionResult> SearchSample([FromQuery(Name = "search-string")] string searchString)
    {
        var (samples, error) = await sampleService.Search(searchString);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (samples == null || samples.Count == 0)
            return NotFound();
        
        return Ok(samples); 
    }
    
    [HttpGet("get-by-playlist")]
    public async Task<IActionResult> GetByPlaylist([FromQuery(Name = "playlist-guid")] Guid playlistGuid)
    {
        var (samples, error) = await sampleService.GetByPlaylist(playlistGuid);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (samples == null || samples.Count == 0)
            return NotFound();
        
        return Ok(samples); 
    }
    
    [HttpGet("get-sample")]
    public async Task<IActionResult> GetSample([FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (sample, error) = await sampleService.GetSample(sampleGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (sample == null)
            return NotFound();
        
        return Ok(sample); 
    }

    [HttpGet("get-user-samples")]
    public async Task<IActionResult> GetUserSamples([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var (sample, error) = await sampleService.GetUserSamples(userGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (sample == null)
            return NotFound();
        
        return Ok(sample); 
    }

    [HttpGet("add-an-listens-to-sample")]
    public async Task<IActionResult> AddAnListensToSample([FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (successfully, error) = await sampleService.AddAnListensToSample(sampleGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
}