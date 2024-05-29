using Microsoft.AspNetCore.Http;
using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface IUserService
{
    public Task<(string? avatarLink, string error)> UploadUserAvatar(Guid userGuid, Stream avatarStream);
    
    public Task<(Guid? userGuid, string error)> SignUp(User newUser);

    public Task<(User? loginUser, string error)> SignIn(HttpResponse response, string nickname, string password);

    public Task<(User? user, string error)> ForgotPassword(string email, string origin);

    public Task<(bool successfully, string error)> ResetPassword(string resetToken, string newPassword);
    
    public Task<(bool successfully, string error)> SignOut(HttpContext context);
    
    public Task<(User? loginUser, string error)> GetUserByNickname(string nickname);
    
    public Task<(User? loginUser, string error)> GetUserByGuid(Guid userGuid);

    public Task<(bool successfully, string error)> EditUser(User user);

    public Task<(bool successfully, string error)> Delete(User user);
}