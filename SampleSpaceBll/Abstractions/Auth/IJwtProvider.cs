using SampleSpaceCore.Models;

namespace SampleSpaceBll.Abstractions.Auth;

public interface IJwtProvider
{
    public Task<string> GenerateToken(User user);
}