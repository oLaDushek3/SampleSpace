using System.Text.Json.Serialization;

namespace SampleSpaceDal.Redis.Entities;

public class Tokens
{
    [JsonPropertyName("user-guid")]
    public Guid UserGuid { get; set; }
    [JsonPropertyName("access-token")]
    public string AccessToken { get; set; } = null!;
    [JsonPropertyName("refresh-token")]
    public string RefreshToken { get; set; } = null!;
}