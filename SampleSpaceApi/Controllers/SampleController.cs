using System.Globalization;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Drawing.Charts;
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
            createdSample.SpotifyLink, createdSample.SoundcloudLink, 0, null);

        if (!string.IsNullOrEmpty(sampleError))
            return BadRequest(sampleError);

        var (sampleGuid, createError) = await sampleService.CreateSample(sample!);

        if (!string.IsNullOrEmpty(createError))
            return BadRequest(createError);

        return Ok(sampleGuid);
    }

    [HttpGet("generate-word")]
    public async Task<IActionResult> GenerateWord([FromQuery(Name = "user-guid")] Guid user_guid)
    {
        var (samples, getError) = await sampleService.GetUserSamples(user_guid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        var doc = new Document();
        var builder = new DocumentBuilder(doc);

        var shape = builder.InsertChart(ChartType.Column, 432, 252);
        shape.Name = "Статистика прослушиваний";
        
        var chart = shape.Chart;

        var seriesColl = chart.Series;

        seriesColl.Clear();

        string[] categories = { "Количество прослушиваний" };

        foreach (var sample in samples!)
        {
            seriesColl.Add(sample.Name, categories, new double[] { sample.NumberOfListens });
        }

        var path = "Files/Statistics.docx";
        doc.Save(path);

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        var resultFile = File(bytes, "application/docx");
        resultFile.FileDownloadName = "Statistics.docx";
        return resultFile;
    }

    [HttpGet("generate-excel")]
    public async Task<IActionResult> GenerateExcel([FromQuery(Name = "user-guid")] Guid user_guid)
    {
        var (samples, getError) = await sampleService.GetUserSamples(user_guid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        var workbook = new Workbook();
        var worksheet = workbook.Worksheets[0];

        worksheet.Cells[0, 0].PutValue("Название");
        worksheet.Cells[1, 0].PutValue("Количество прослушиваний");
        for (var i = 0; i < samples!.Count; i++)
        {
            worksheet.Cells[0, i + 1].PutValue(samples[i].Name);
            worksheet.Cells[1, i + 1].PutValue(samples[i].NumberOfListens);
        }

        var chartIndex = worksheet.Charts.Add(Aspose.Cells.Charts.ChartType.Column, 3, 0, 13, 3 + samples.Count);
        var chart = worksheet.Charts[chartIndex];

        chart.SetChartDataRange($"A1:{worksheet.Cells[1, samples.Count].Name}", true);

        var path = "Files/Statistics.xls";

        workbook.Save(path);

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        var resultFile = File(bytes, "application/xls");
        resultFile.FileDownloadName = "Statistics.xls";
        return resultFile;
    }
}