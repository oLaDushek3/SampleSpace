using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Abstractions.AuthScheme;
using SampleSpaceBll.Models;

namespace SampleSpaceInfrastructure.AuthScheme.Cookie;

public class CookieManager : ICookieManager
{
    private CookieOptions GetOptions(DateTime expires) =>
        new (){Expires = expires, Secure = false};
    
    private CookieOptions DefaultOptions() =>
        new (){Secure = false};
    
    public Tokens? GetTokens(HttpContext context)
    {
        var accessToken = context.Request.Cookies[CookiesList.AccessToken];

        var refreshToken = context.Request.Cookies[CookiesList.RefreshToken];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return null;

        return new Tokens { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public void SetTokens(HttpContext context, Tokens tokens, int refreshTokenLifeTimeInDays)
    {
        var expires = DateTime.UtcNow.AddDays(refreshTokenLifeTimeInDays);

        var options = GetOptions(expires);

        context.Response.Cookies.Append(CookiesList.AccessToken, tokens.AccessToken, options);
        context.Response.Cookies.Append(CookiesList.RefreshToken, tokens.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        var options = DefaultOptions();
        
        context.Response.Cookies.Delete(CookiesList.AccessToken, options);
        context.Response.Cookies.Delete(CookiesList.RefreshToken, options);
    }
}