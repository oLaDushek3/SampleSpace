using System.Globalization;
using System.Security.Claims;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.Sample;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models.Sample;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/sample")]
public class SampleController(ISampleService sampleService) : ControllerBase
{
    [EnableCors]
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

    [HttpPost("create-sample")]
    [RequestSizeLimit(20_000_000)]
    public async Task<IActionResult> CreateSample([FromForm] CreateSampleRequest createSampleRequest)
    {
        var sampleStart = double.Parse(createSampleRequest.SampleStart, CultureInfo.InvariantCulture);
        var sampleEnd = double.Parse(createSampleRequest.SampleEnd, CultureInfo.InvariantCulture);

        var (createdSample, createdSampleError) = CreatedSample.Create(Guid.NewGuid(),
            createSampleRequest.SampleFile.OpenReadStream(), sampleStart,
            sampleEnd, createSampleRequest.CoverFile.OpenReadStream(), createSampleRequest.Name,
            createSampleRequest.Artist, createSampleRequest.UserGuid, createSampleRequest.VkontakteLink,
            createSampleRequest.SpotifyLink, createSampleRequest.SoundcloudLink);

        if (!string.IsNullOrEmpty(createdSampleError))
            return BadRequest(createdSampleError);

        var (trimmedSampleStream, trimmedError) = sampleService.TrimSample(
            createdSample!.SampleStream,
            sampleStart,
            sampleEnd);

        if (!string.IsNullOrEmpty(trimmedError))
        {
            createdSample.Dispose();
            return BadRequest(trimmedError);
        }

        var (sampleLink, coverLink, uploadError) = await sampleService.UploadSample(
            createdSample.UserGuid,
            createdSample.Name,
            trimmedSampleStream!,
            createdSample.CoverStream);

        createdSample.Dispose();

        if (!string.IsNullOrEmpty(uploadError))
            return BadRequest(uploadError);

        var (sample, sampleError) = Sample.Create(Guid.NewGuid(), sampleLink!, coverLink!, createdSample.Name,
            createdSample.Artist, createdSample.UserGuid, (sampleEnd - sampleStart), createdSample.VkontakteLink,
            createdSample.SpotifyLink, createdSample.SoundcloudLink, 0, null, DateOnly.FromDateTime(DateTime.Now));

        if (!string.IsNullOrEmpty(sampleError))
            return BadRequest(sampleError);

        var (sampleGuid, createError) = await sampleService.CreateSample(sample!);

        if (!string.IsNullOrEmpty(createError))
            return BadRequest(createError);

        return Ok(sampleGuid);
    }

    [Authorize]
    [HttpDelete("delete-sample")]
    [RequestSizeLimit(20_000_000)]
    public async Task<IActionResult> DeleteSample([FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (sample, getError) = await sampleService.GetSample(sampleGuid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        var loginUserGuid = Guid.Parse(User.FindFirst(ClaimTypes.Authentication)!.Value);
        var userIsAdmin = Convert.ToBoolean(User.FindFirst(ClaimTypes.Role)!.Value);

        if (loginUserGuid != sample!.UserGuid && !userIsAdmin)
            return Forbid();

        var (successfully, deleteError) = await sampleService.DeleteSample(sample);

        if (!string.IsNullOrEmpty(deleteError))
            return BadRequest(deleteError);

        return successfully ? Ok() : BadRequest("Server error");
    }
}