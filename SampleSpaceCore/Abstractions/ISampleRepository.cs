using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface ISampleRepository
{
    public Task<List<Sample>> GetAll();

    public Task<List<Sample>> Search(string searchString);
}