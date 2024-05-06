namespace SampleSpaceApi.Contracts.SampleComment;

public record EditSampleCommentRequest(
    Guid CommentGuid,
    string Comment);