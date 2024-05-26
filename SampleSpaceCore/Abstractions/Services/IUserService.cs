using Microsoft.AspNetCore.Http;
using SampleSpaceCore.Models.User;

namespace SampleSpaceCore.Abstractions.Services;

public interface IUserService
{
    public Task<(string? avatarLink, string error)> UploadUserAvatar(Guid userGuid, Stream avatarStream);
    
    public Task<(Guid? userGuid, string error)> SignUp(User newUser);

    public Task<(User? loginUser, string error)> SignIn(HttpResponse response, string nickname, string password);

    public Task<(bool successfully, string error)> SignOut(HttpContext context);
    
    public Task<(User? loginUser, string error)> GetUser(string nickname);
}