using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class SampleService(ISampleRepository sampleRepository) : ISampleService
{
    public Task<List<Sample>> GetAll()
    {
        return sampleRepository.GetAll();
    }
}