using System.Net;
using System.Net.Http.Headers;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;

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
    public async Task<IActionResult> GetAllSamples([FromQuery(Name = "search_string")] string searchString)
    {
        var samples = await sampleService.Search(searchString);
        return Ok(samples);
    }

    [HttpGet("GetUserSamples")]
    public async Task<IActionResult> GetUserSamples([FromQuery(Name = "user_guid")] Guid userGuid)
    {
        var samples = await sampleService.GetUserSamples(userGuid);
        return Ok(samples);
    }

    [HttpGet("AddAnListensToSample")]
    public async Task<IActionResult> AddAnListensToSample([FromQuery(Name = "sample_guid")] Guid sampleGuid)
    {
        var result = await sampleService.AddAnListensToSample(sampleGuid);
        return result ? Ok() : BadRequest("Invalid data");
    }

    [HttpGet("GenerateWord")]
    public async Task<IActionResult> GenerateWord([FromQuery(Name = "user_guid")] Guid user_guid)
    {
        var samples = await sampleService.GetUserSamples(user_guid);

        var doc = new Document();
        var builder = new DocumentBuilder(doc);

        var shape = builder.InsertChart(ChartType.Column, 432, 252);

        var chart = shape.Chart;

        var seriesColl = chart.Series;

        seriesColl.Clear();

        string[] categories = { "Количество прослушиваний" };

        foreach (var sample in samples)
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

    [HttpGet("GenerateExcel")]
    public async Task<IActionResult> GenerateExcel([FromQuery(Name = "user_guid")] Guid user_guid)
    {
        var samples = await sampleService.GetUserSamples(user_guid);

        var workbook = new Workbook();
        var worksheet = workbook.Worksheets[0];

        worksheet.Cells[1, 0].PutValue("Количество прослушиваний");
        for (var i = 0; i < samples.Count; i++)
        {
            worksheet.Cells[0, i].PutValue(samples[i].Name);
            worksheet.Cells[1, i].PutValue(samples[i].NumberOfListens);
        }

        var chartIndex = worksheet.Charts.Add(Aspose.Cells.Charts.ChartType.Column, 3, 0, 13, 3 + samples.Count);
        var chart = worksheet.Charts[chartIndex];

        chart.SetChartDataRange($"A1:{worksheet.Cells[1, samples.Count + 1].Name}", true);
        
        var path = "Files/Statistics.xls";

        workbook.Save(path);

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        var resultFile = File(bytes, "application/xls");
        resultFile.FileDownloadName = "Statistics.xls";
        return resultFile;
    }
}