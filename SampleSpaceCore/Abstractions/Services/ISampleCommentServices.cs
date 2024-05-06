using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Services;

public interface ISampleCommentServices
{
    public Task<(SampleComment?, string error)> GetSampleComment(Guid commentGuid);
    
    public Task<(List<SampleComment>?, string error)> GetSampleComments(Guid sampleGuid);
    
    public Task<(Guid? commentGuid, string error)> CreateNewComment(SampleComment comment);

    public Task<(bool successfully, string error)> EditComment(SampleComment comment);
    
    public Task<(bool successfully, string error)> DeleteComment(Guid commentGuid);
}