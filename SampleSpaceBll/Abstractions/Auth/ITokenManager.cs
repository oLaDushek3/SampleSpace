using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Models;

namespace SampleSpaceBll.Abstractions.Auth;

public interface ITokenManager
{
    public Tokens CreateTokens(Guid userGuid);

    public Task SaveTokens(HttpResponse response, Tokens tokens, Guid userId);
    
    public Task RefreshTokens(HttpResponse response, Tokens tokens);

    public Task DeleteTokens(HttpContext context);
    
    public ClaimsPrincipal GetPrincipalFromToken(string token);

    public Guid GetUserIdFromToken(string token);
    
    public bool CheckAccessTokenValid(string token);

    public bool CheckAccessTokenActive(string token);
}