using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface IUserService
{
    public Task<(Guid? userGuid, string error)> SigUp(User newUser);

    public Task<(User? loginUser, string? token, string error)> SigIn(string nickname, string password);
    
    public Task<(User? loginUser, string error)> GetUser(string nickname);
}