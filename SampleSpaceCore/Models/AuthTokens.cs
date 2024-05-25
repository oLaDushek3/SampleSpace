namespace SampleSpaceCore.Models;

public class AuthTokens
{
    private AuthTokens(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    
    public string AccessToken { get; private set; }

    public string RefreshToken { get; private set; }

    public static (AuthTokens? tokens, string error) Create(string accessToken, string refreshToken)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return (null, "Tokens can not be empty");

        var tokens = new AuthTokens(accessToken, refreshToken);

        return (tokens, string.Empty);
    }
}