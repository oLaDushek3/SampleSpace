namespace SampleSpaceApi.Contracts.Playlist;

public record AddSampleToPlaylistRequest(
    Guid PlaylistGuid,
    Guid SampleGuid);