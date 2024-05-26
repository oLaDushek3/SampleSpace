using SampleSpaceCore.Models;
using SampleSpaceCore.Models.User;

namespace SampleSpaceBll.Abstractions.Auth;

public interface IJwtProvider
{
    public Task<string> GenerateToken(User user);
}