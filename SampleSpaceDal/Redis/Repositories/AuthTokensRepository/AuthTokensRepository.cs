using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SampleSpaceCore.Abstractions.Redis.Repositories;
using SampleSpaceCore.Models;
using SampleSpaceDal.Redis.Entities;

namespace SampleSpaceDal.Redis.Repositories.AuthTokensRepository;

public class AuthTokensRepository(IDistributedCache cache) : IAuthTokensRepository
{
    private string Key(Guid userId, string refreshToken) => $"{userId}:{refreshToken}";
    
    public async Task<AuthTokens?> GetTokens(Guid userGuid, string refreshToken)
    {
        var key = Key(userGuid, refreshToken);
        
        var tokens = await cache.GetStringAsync(key);

        if (tokens == null)
            return null;

        var token = JsonSerializer.Deserialize<Tokens>(tokens)!;

        var (authTokens, error) = AuthTokens.Create(token.AccessToken, token.RefreshToken);
        
        return authTokens;
    }

    public async Task SetTokens(Guid userGuid, AuthTokens authTokens, TimeSpan refreshTokenLifeTime)
    {
        var tokenLifeTime = DateTime.UtcNow.AddDays(refreshTokenLifeTime.TotalDays);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = new DateTimeOffset(tokenLifeTime)
        };

        var tokens = new Tokens
        {
            UserGuid = userGuid,
            AccessToken = authTokens.AccessToken,
            RefreshToken = authTokens.RefreshToken
        };
            
        var key = Key(userGuid, tokens.RefreshToken);

        var value = JsonSerializer.Serialize(tokens);

        await cache.SetStringAsync(key, value, options);
    }

    public async Task DeleteTokens(Guid userGuid, string refreshToken)
    {
        var key = Key(userGuid, refreshToken);

        await cache.RemoveAsync(key);
    }
}