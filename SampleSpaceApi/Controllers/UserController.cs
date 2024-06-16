using System.Security.Claims;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.User;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceDal.PostgreSQL.Repositories.SampleRepository;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService, ISampleService sampleService) : ControllerBase
{
    [HttpPost("sign-up")]
    [RequestSizeLimit(7_500_000)]
    public async Task<IActionResult> SigUp([FromForm] SignUpUserRequest request)
    {
        var existUser = await userService.GetUserByNickname(request.Nickname);

        if (existUser.loginUser != null)
            return Conflict("User with this nickname already exists");

        existUser = await userService.GetUserByEmail(request.Email);

        if (existUser.loginUser != null)
            return Conflict("User with this email already exists");

        var (passwordValid, passwordValidError) = userService.PasswordValidation(request.Password);

        if (!passwordValid)
            return BadRequest(passwordValidError);

        var (user, userError) = SampleSpaceCore.Models.User.Create(Guid.NewGuid(), request.Nickname,
            request.Email, request.Password);

        if (!string.IsNullOrEmpty(userError))
            return BadRequest(userError);

        if (request.AvatarFile != null)
        {
            var avatarStream = request.AvatarFile.OpenReadStream();

            var (avatarLink, uploadError) = await userService.UploadUserAvatar(user!.UserGuid, avatarStream);

            if (!string.IsNullOrEmpty(uploadError))
            {
                await avatarStream.DisposeAsync();
                return BadRequest(uploadError);
            }

            user.PutAvatarPath(avatarLink!);
        }

        var (signUpUserGuid, signUpError) = await userService.SignUp(user!);

        if (!string.IsNullOrEmpty(signUpError))
            return BadRequest(signUpError);

        return Ok();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInUserRequest request)
    {
        var (loginUser, error) = await userService.SignIn(Response, request.Nickname, request.Password);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return Ok(new UserResponse(loginUser!.UserGuid, loginUser.AvatarPath, loginUser.Nickname, loginUser.Email,
            loginUser.IsAdmin));
    }

    [HttpPost("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        var (successfully, error) = await userService.SignOut(HttpContext);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return successfully ? Ok() : BadRequest("Server error");
    }

    [Authorize]
    [HttpPut("edit-user")]
    public async Task<IActionResult> EditUser([FromForm] EditUserRequest request)
    {
        var (user, getError) = await userService.GetUserByGuid(request.UserGuid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;

        if (Guid.Parse(loginUserGuid) != request.UserGuid)
            return Forbid();

        var modified = user!.Edit(request.Nickname, request.Email);

        if (!string.IsNullOrEmpty(modified.Error))
            return BadRequest(modified.Error);

        if (request.AvatarFile != null)
        {
            var avatarStream = request.AvatarFile.OpenReadStream();

            var (avatarLink, uploadError) = await userService.UploadUserAvatar(user.UserGuid, avatarStream);

            if (!string.IsNullOrEmpty(uploadError))
            {
                await avatarStream.DisposeAsync();
                return BadRequest(uploadError);
            }

            user.PutAvatarPath(avatarLink!);
        }

        var (successfully, editError) = await userService.EditUser(user);

        if (!string.IsNullOrEmpty(editError))
            return BadRequest(editError);

        return successfully ? Ok(user) : BadRequest("Server error");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var (user, error) = await userService.ForgotPassword(request.Email, request.Route);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return user != null ? Ok() : NotFound("User not found");
    }

    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var (successfully, error, errorCode) = await userService.ResetPassword(request.ResetToken, request.NewPassword);

        if (errorCode == 400)
            return BadRequest("Server error");

        if (errorCode != 200)
            return Forbid();

        return successfully ? Ok() : BadRequest($"Server error: {error}");
    }

    [Authorize]
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var (user, getError) = await userService.GetUserByGuid(userGuid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        var loginUserGuid = Guid.Parse(User.FindFirst(ClaimTypes.Authentication)!.Value);
        var userIsAdmin = Convert.ToBoolean(User.FindFirst(ClaimTypes.Role)!.Value);

        if (loginUserGuid != userGuid && !userIsAdmin)
            return Forbid();

        var (successfully, deleteError) = await userService.Delete(user!);

        if (!string.IsNullOrEmpty(deleteError))
            return BadRequest(deleteError);

        return successfully ? Ok() : BadRequest("Server error");
    }

    [HttpGet("get-user-by-nickname")]
    public async Task<IActionResult> GetUser(string nickname)
    {
        var (user, error) = await userService.GetUserByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return Ok(user);
    }

    [Authorize]
    [HttpGet("generate-word")]
    public async Task<IActionResult> GenerateWord([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var userIsAdmin = Convert.ToBoolean(User.FindFirst(ClaimTypes.Role)!.Value);

        if (!userIsAdmin)
            return Forbid();
        
        var (samples, getError) = await sampleService.GetUserSamples(userGuid, 0, 0);

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
    
    [Authorize]
    [HttpGet("generate-excel")]
    public async Task<IActionResult> GenerateExcel([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var userIsAdmin = Convert.ToBoolean(User.FindFirst(ClaimTypes.Role)!.Value);

        if (!userIsAdmin)
            return Forbid();
        
        var (samples, getError) = await sampleService.GetUserSamples(userGuid, 0, 0);

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

    [Authorize]
    [HttpGet("get-sample-addition-statistics")]
    public async Task<IActionResult> GetSampleAdditionStatistics([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var userIsAdmin = Convert.ToBoolean(User.FindFirst(ClaimTypes.Role)!.Value);

        if (!userIsAdmin)
            return Forbid();
        
        var (sampleAdditionStatistics, generateError) = await userService.GenerateSampleAdditionStatistics(userGuid);

        if (!string.IsNullOrEmpty(generateError))
            return BadRequest(generateError);

        return Ok(sampleAdditionStatistics);
    }
}