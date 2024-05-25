using Microsoft.AspNetCore.Authentication;

namespace SampleSpaceInfrastructure.AuthScheme;

public static class AuthTokensExtensions
{
    public static AuthenticationBuilder AddAuthTokens(
        this AuthenticationBuilder builder, 
        string authenticationScheme,
        Action<AuthTokensOptions> configureOptions)
        => builder.AddScheme<AuthTokensOptions, AuthTokensHandler>(authenticationScheme, displayName: null, configureOptions);
}