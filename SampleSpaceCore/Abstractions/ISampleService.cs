using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleService
{
    public Task<List<Sample>> GetAll();

    public Task<List<Sample>> Search(string searchString);
}