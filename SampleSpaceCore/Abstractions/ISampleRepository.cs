using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleRepository
{
    public Task<List<Sample>> GetAll();
}