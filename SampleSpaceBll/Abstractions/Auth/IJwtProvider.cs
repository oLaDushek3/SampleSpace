using SampleSpaceCore.Models;

namespace SampleSpaceBll.Abstractions.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}