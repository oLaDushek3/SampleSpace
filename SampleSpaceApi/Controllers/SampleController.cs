using Microsoft.AspNetCore.Mvc;
using SampleSpaceCore.Abstractions;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController(ISampleService sampleService) : ControllerBase
{
    [HttpGet("GetAllSamples")]
    public async Task<IActionResult> GetAllSamples()
    {
        var samples = await sampleService.GetAll();
        return Ok(samples);
    }
    
    [HttpGet("SearchSamples")]
    public async Task<IActionResult> GetAllSamples([FromQuery(Name = "search_string")]string searchString)
    {
        var samples = await sampleService.Search(searchString);
        return Ok(samples);
    }
}