using Microsoft.AspNetCore.Authentication;

namespace SampleSpaceInfrastructure.AuthScheme;

public class AuthTokensOptions : AuthenticationSchemeOptions
{
    #region Validate

    public bool ValidateIssuer { get; set; }
    
    public bool ValidateAudience { get; set; }
    
    public bool ValidateIssuerSigningKey { get; set; }

    #endregion

    #region ValidationParams

    public string Issuer { get; set; } = null!;

    public string IssuerSigningKey { get; set; } = null!;
    
    public string Audience { get; set; } = null!;

    #endregion

    #region TokensLifeTimes

    public int RefreshTokenExpiresInDays { get; set; }

    public int AccessTokenExpiresInMinutes { get; set; }
    
    public int ResetTokenExpiresInHours { get; set; }

    #endregion
    
    public TimeSpan RefreshTokenExpires => new(RefreshTokenExpiresInDays, 0, 0, 0, 0, 0);
}