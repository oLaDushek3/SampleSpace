using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleService
{
    public Task<List<Sample>> GetAll();
}