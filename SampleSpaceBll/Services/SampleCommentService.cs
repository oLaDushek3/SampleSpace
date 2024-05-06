using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class SampleCommentService(ISampleCommentRepository sampleCommentRepository) : ISampleCommentServices
{
    public async Task<(SampleComment?, string error)> GetSampleComment(Guid commentGuid)
    {
        return await sampleCommentRepository.GetByGuid(commentGuid);
    }
    
    public async Task<(List<SampleComment>?, string error)> GetSampleComments(Guid sampleGuid)
    {
        return await sampleCommentRepository.Get(sampleGuid);
    }
    
    public async Task<(Guid? commentGuid, string error)> CreateNewComment(SampleComment comment)
    {
        return await sampleCommentRepository.Create(comment);
    }

    public async Task<(bool successfully, string error)> EditComment(SampleComment comment)
    {
        return await sampleCommentRepository.Edit(comment);
    }

    public async Task<(bool successfully, string error)> DeleteComment(Guid commentGuid)
    {
        return await sampleCommentRepository.Delete(commentGuid);
    }
}