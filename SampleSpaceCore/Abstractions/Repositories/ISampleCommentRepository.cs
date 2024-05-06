using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Repositories;

public interface ISampleCommentRepository
{
    public Task<(SampleComment?, string error)> GetByGuid(Guid commentGuid);

    public Task<(List<SampleComment>?, string error)> Get(Guid sampleGuid);
    
    public Task<(Guid? commentGuid, string error)> Create(SampleComment comment);
    
    public Task<(bool successfully, string error)> Edit(SampleComment comment);
    
    public Task<(bool successfully, string error)> Delete(Guid commentGuid);
}