using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceCore.Models;

namespace SampleSpaceInfrastructure.JWT;

public class JwtProvider(IOptions<JwtOptions> options, IDistributedCache cache) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    private string Key(Guid userGuid, string refreshToken) => $"{userGuid}:{refreshToken}";
    
    public async Task<string> GenerateToken(User user)
    {
        // var key = Key(user.UserGuid, user.Nickname);
        //
        // var value = JsonSerializer.Serialize(user.Nickname);
        //
        // //var result = await _cache.GetStringAsync(user.Nickname);
        //
        // await cache.SetStringAsync(key, value);
        
        Claim[] claims = { new(ClaimTypes.Authentication, user.UserGuid.ToString()) };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}