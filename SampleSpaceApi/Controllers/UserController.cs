﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.User;
using SampleSpaceCore.Abstractions;
using SampleSpaceInfrastructure;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("SignUp")]
    public async Task<IActionResult> SigUp(UserRequest request)
    {
        var (newUser, error) =
            SampleSpaceCore.Models.User.Create(Guid.NewGuid(), request.Nickname, request.Email, request.Password);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        var userGuid = await userService.SigUp(newUser);

        return Ok(userGuid);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(LoginUserRequest request)
    {
        var test = new s3test();
        var r = test.Main();
        
        var (loginUser, token, error) = await userService.SigIn(request.Nickname, request.Password);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        HttpContext.Response.Cookies.Append("jwt", token!);
        
        return Ok(loginUser);
    }
    
    [HttpPost("GetUser")]
    public async Task<IActionResult> GetUser(string nickname)
    {
        var test = new s3test();
        var r = test.Main();
        
        var (user, error) = await userService.GetUser(nickname);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        return Ok(user);
    }
    
    [Authorize]
    [HttpPost("SigInAuth")]
    public async Task<IActionResult> SigInAuth(LoginUserRequest request)
    {
        var user = User;
        var id = user.FindFirst(ClaimTypes.Authentication)?.Value;
        return Ok(id);
    }
}