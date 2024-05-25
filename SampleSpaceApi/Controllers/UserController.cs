using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.User;
using SampleSpaceCore.Abstractions.Services;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SigUp(UserRequest request)
    {
        var (requestUser, requestError) =
            SampleSpaceCore.Models.User.Create(Guid.NewGuid(), request.Nickname, request.Email, request.Password);

        if (!string.IsNullOrEmpty(requestError))
            return BadRequest(requestError);

        var (signUpUserGuid, signUpError) = await userService.SignUp(requestUser!);

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