using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SampleSpaceBll.Abstractions.AuthScheme;
using SampleSpaceBll.Models;
using SampleSpaceCore.Abstractions.Redis.Repositories;
using SampleSpaceCore.Models;

namespace SampleSpaceInfrastructure.AuthScheme.Token;

public class TokenManager(AuthTokensOptions options, IAuthTokensRepository authTokensRepository,
    ICookieManager cookieManager) : ITokenManager
{
    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)),
            ValidateLifetime = false,
            ValidateAudience = options.ValidateAudience,
            ValidateIssuer = options.ValidateIssuer,
            ValidAudience = options.Audience,
            ValidIssuer = options.Issuer
        };
    }

    public string CreateResetToken(Guid userGuid)
    {
        Claim[] claims = { new(ClaimTypes.Authentication, userGuid.ToString()) };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(options.ResetTokenExpiresInHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
    
    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(options.AccessTokenExpiresInMinutes));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public Tokens CreateTokens(Guid userGuid)
    {
        Claim[] claims = { new(ClaimTypes.Authentication, userGuid.ToString()) };

        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();

        return new Tokens { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task SaveTokens(HttpResponse response, Tokens tokens, Guid userId)
    {
        var (authTokens, error) = AuthTokens.Create(tokens.AccessToken, tokens.RefreshToken);

        await authTokensRepository.SetTokens(userId, authTokens!, options.RefreshTokenExpires);

        cookieManager.SetTokens(response.HttpContext, tokens, options.RefreshTokenExpiresInDays);
    }

    public async Task RefreshTokens(HttpResponse response, Tokens tokens)
    {
        var userId = GetUserIdFromToken(tokens.AccessToken);

        var cachedTokens = await authTokensRepository.GetTokens(userId, tokens.RefreshToken);

        if (cachedTokens == null)
            throw new Exception("Tokens in cache not found");

        await authTokensRepository.DeleteTokens(userId, cachedTokens.RefreshToken);

        var newTokens = CreateTokens(userId);

        await SaveTokens(response, newTokens, userId);
    }

    public async Task DeleteTokens(HttpContext context)
    {
        var tokens = cookieManager.GetTokens(context);

        cookieManager.DeleteTokens(context);

        if (tokens == null)
            return;

        var userId = GetUserIdFromToken(tokens.AccessToken);

        await authTokensRepository.DeleteTokens(userId, tokens.RefreshToken);
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = GetValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        return claims;
    }

    public Guid GetUserIdFromToken(string token)
    {
        var claims = GetPrincipalFromToken(token);

        var guidString = claims.FindFirst(ClaimTypes.Authentication)!.Value;

        return Guid.Parse(guidString);
    }

    public bool CheckTokenValid(string token)
    {
        var tokenValidationParameters = GetValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))

                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CheckTokenActive(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = handler.ReadJwtToken(token);

        return jwtSecurityToken.ValidTo >= DateTime.UtcNow;
    }
}