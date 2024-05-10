namespace SampleSpaceApi.Contracts.Playlist;

public record CreatePlaylistRequest(
    Guid UserGuid,
    string Name);