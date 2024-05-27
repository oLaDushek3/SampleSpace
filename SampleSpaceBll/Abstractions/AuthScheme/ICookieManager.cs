using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Models;

namespace SampleSpaceBll.Abstractions.AuthScheme;

public interface ICookieManager
{
    public Tokens? GetTokens(HttpContext context);

    public void SetTokens(HttpContext context, Tokens tokensDomain, int refreshTokenLifeTimeInDays);

    public void DeleteTokens(HttpContext context);
}