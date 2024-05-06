namespace SampleSpaceApi.Contracts.SampleComment;

public record CreateSampleCommentRequest(
    Guid SampleGuid,
    Guid UserGuid,
    string Comment);