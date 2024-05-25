using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Redis.Repositories;

public interface IAuthTokensRepository
{
    public Task<AuthTokens?> GetTokens(Guid userGuid, string refreshToken);
    
    public Task SetTokens(Guid userGuid, AuthTokens tokens, TimeSpan refreshTokenLifeTime);
    
    public Task DeleteTokens(Guid userGuid, string refreshToken);
}