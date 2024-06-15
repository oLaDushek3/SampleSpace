using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Models;

namespace SampleSpaceBll.Abstractions.AuthScheme;

public interface ITokenManager
{
    public string CreateResetToken(Guid userGuid);

    public Tokens CreateTokens(UserClaims userClaims);

    public Task SaveTokens(HttpResponse response, Tokens tokens, Guid userId);
    
    public Task RefreshTokens(HttpResponse response, Tokens tokens);

    public Task DeleteTokens(HttpContext context);
    
    public ClaimsPrincipal GetPrincipalFromToken(string token);

    public UserClaims GetUserClaimsFromToken(string token);
    
    public bool CheckTokenValid(string token);

    public bool CheckTokenActive(string token);
}