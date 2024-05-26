using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.User;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models.User;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("sign-up")]
    [RequestSizeLimit(7_500_000)]
    public async Task<IActionResult> SigUp([FromForm] UserRequest request)
    {
        var (existUser, existError) = await userService.GetUser(request.Nickname);

        if (existUser != null)
            return Conflict("User with this nickname already exists");
        
        var (createdUser, createError) =
            CreatedUser.Create(Guid.NewGuid(), request.Nickname, request.Email, request.Password,
                request.AvatarFile?.OpenReadStream());
        
        if (!string.IsNullOrEmpty(createError))
            return BadRequest(createError);

        var (user, userError) = SampleSpaceCore.Models.User.User.Create(createdUser!.UserGuid, createdUser.Nickname,
            createdUser.Email, createdUser.Password);
        
        if (!string.IsNullOrEmpty(userError))
            return BadRequest(userError);
        
        if (createdUser.AvatarStream != null)
        {
            var (avatarLink, uploadError) = await userService.UploadUserAvatar(createdUser.UserGuid, createdUser.AvatarStream);

            if (!string.IsNullOrEmpty(uploadError))
            {
                createdUser.Dispose();
                return BadRequest(uploadError);
            }

            user!.PutAvatarPath(avatarLink!);
        }
        
        var (signUpUserGuid, signUpError) = await userService.SignUp(user!);

        if (!string.IsNullOrEmpty(signUpError))
            return BadRequest(signUpError);

        return Ok(signUpUserGuid);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(LoginUserRequest request)
    {
        var (loginUser, error) = await userService.SignIn(Response, request.Nickname, request.Password);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        HttpContext.Response.Cookies.Append("jwt", "!test!");

        return Ok(new UserResponse(loginUser!.UserGuid, loginUser.AvatarPath, loginUser.Nickname, loginUser.Email));
    }

    [HttpPost("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        var (successfully, error) = await userService.SignOut(HttpContext);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return Ok();
    }

    [HttpGet("get-user-by-nickname")]
    public async Task<IActionResult> GetUser(string nickname)
    {
        var (user, error) = await userService.GetUser(nickname);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return Ok(user);
    }

    [Authorize]
    [HttpGet("sig-in-auth")]
    public async Task<IActionResult> SigInAuth()
    {
        var test = HttpContext.Request.Cookies["jwt"];
        return Ok(test);
        var id = User.FindFirst(ClaimTypes.Authentication)?.Value;
        return Ok(id);
    }
}