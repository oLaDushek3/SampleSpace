using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface IUserService
{
    public Task<Guid> SigUp(User newUser);

    public Task<(string? token, string error)> SigIn(string nickname, string password);
}