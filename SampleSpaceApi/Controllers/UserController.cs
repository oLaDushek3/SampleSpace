using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.User;
using SampleSpaceCore.Abstractions.Services;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("sign-up")]
    [RequestSizeLimit(7_500_000)]
    public async Task<IActionResult> SigUp([FromForm] SignUpUserRequest request)
    {
        var existUser = await userService.GetUserByNickname(request.Nickname);

        if (existUser.loginUser != null)
            return Conflict("User with this nickname already exists");

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

        return Ok(signUpUserGuid);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInUserRequest request)
    {
        var (loginUser, error) = await userService.SignIn(Response, request.Nickname, request.Password);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return Ok(new UserResponse(loginUser!.UserGuid, loginUser.AvatarPath, loginUser.Nickname, loginUser.Email));
    }

    //[Authorize]
    [HttpPost("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        var (successfully, error) = await userService.SignOut(HttpContext);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);

        return successfully ? Ok() : BadRequest("Server error");
    }

    //[Authorize]
    [HttpPut("edit-user")]
    public async Task<IActionResult> EditUser([FromForm] EditUserRequest request)
    {
        var (user, getError) = await userService.GetUserByGuid(request.UserGuid);

        if (!string.IsNullOrEmpty(getError))
            return BadRequest(getError);

        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (Guid.Parse(loginUserGuid) != request.UserGuid)
        //     return Forbid();
        
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
        
        if(!string.IsNullOrEmpty(editError))
            return  BadRequest(editError);

        return successfully ? Ok(user) : BadRequest("Server error");
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var (successfully, error) = await userService.ForgotPassword(request.Email, request.Route);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
    
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var (successfully, error) = await userService.ResetPassword(request.ResetToken, request.NewPassword);

        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
    
    //[Authorize]
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser([FromQuery(Name = "user-guid")] Guid userGuid)
    {
        var (user, getError) = await userService.GetUserByGuid(userGuid);
        
        // if(!string.IsNullOrEmpty(getError))
        //     return  BadRequest(getError);
        
        var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        
        if (Guid.Parse(loginUserGuid) != userGuid)
            return Forbid();
        
        var (successfully, deleteError) = await userService.Delete(user!);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
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
}