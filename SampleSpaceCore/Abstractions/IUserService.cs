using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface IUserService
{
    public Task<Guid> SigUp(User newUser);

    public Task<(User? loginUser, string? token, string error)> SigIn(string nickname, string password);
    
    public Task<(User? loginUser, string error)> GetUser(string nickname);
}