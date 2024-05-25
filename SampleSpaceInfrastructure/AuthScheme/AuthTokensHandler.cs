using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleSpaceBll.Abstractions.Auth;

namespace SampleSpaceInfrastructure.AuthScheme;

public class AuthTokensHandler : AuthenticationHandler<AuthTokensOptions>
{
    private readonly ICookieManager _cookieManager;
    private readonly ITokenManager _tokenManager;
    
    public AuthTokensHandler(IOptionsMonitor<AuthTokensOptions> options, ILoggerFactory logger, UrlEncoder encoder, ICookieManager cookieManager, ITokenManager tokenManager) :
        base(options, logger, encoder)
    {
        _cookieManager = cookieManager;
        _tokenManager = tokenManager;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var tokens = _cookieManager.GetTokens(Request.HttpContext);
        
        if(tokens == null)
            return AuthenticateResult.Fail("Tokens Not Found");

        if (_tokenManager.CheckAccessTokenValid(tokens.AccessToken))
        {
            var accessTokenIsActive = _tokenManager.CheckAccessTokenActive(tokens.AccessToken);
            var principal = _tokenManager.GetPrincipalFromToken(tokens.AccessToken);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            if (accessTokenIsActive)
                return AuthenticateResult.Success(ticket);
            
            try
            {
                await _tokenManager.RefreshTokens(Response, tokens);
                
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Refresh token not active");
            }
        }
        
        return AuthenticateResult.Fail("Invalid token");
    }
}