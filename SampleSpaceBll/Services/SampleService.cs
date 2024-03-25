using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class SampleService(ISampleRepository sampleRepository) : ISampleService
{
    public async Task<List<Sample>> GetAll()
    {
        return await sampleRepository.GetAll();
    }
    
    public async Task<List<Sample>> Search(string searchString)
    {
        return await sampleRepository.Search(searchString);
    }
}