namespace SampleSpaceApi.Contracts.Sample;

public record CreateSampleRequest(
    IFormFile SampleFile,
    string SampleStart,
    string SampleEnd,
    IFormFile CoverFile,
    string Name, 
    string Artist, 
    Guid UserGuid, 
    string? VkontakteLink, 
    string? SpotifyLink, 
    string? SoundcloudLink);