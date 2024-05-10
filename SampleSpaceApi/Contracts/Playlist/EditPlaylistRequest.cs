namespace SampleSpaceApi.Contracts.Playlist;

public record EditPlaylistRequest(
    Guid PlaylistGuid,
    string Name);