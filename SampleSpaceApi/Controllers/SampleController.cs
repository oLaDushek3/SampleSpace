using Microsoft.AspNetCore.Mvc;
using SampleSpaceCore.Abstractions;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController(ISampleService sampleService) : ControllerBase
{
    [HttpGet("GetAllSamples")]
    public async Task<IActionResult> GetAllSamples()
    {
        var samples = await sampleService.GetAll();
        return Ok(samples);
    }
}