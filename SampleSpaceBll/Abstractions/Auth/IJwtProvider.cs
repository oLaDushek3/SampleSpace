using SampleSpaceCore.Models;

namespace SampleSpaceBll.Abstractions.Auth;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}