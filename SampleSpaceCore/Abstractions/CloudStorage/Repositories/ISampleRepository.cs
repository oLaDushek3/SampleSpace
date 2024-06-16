namespace SampleSpaceCore.Abstractions.CloudStorage.Repositories;

public interface ISampleRepository
{
    public Task<(string? sampleLink, string? coverLink, string error)> Create(Guid userGuid, string sampleName,
        Stream sampleStream, string sampleFileExtension, Stream coverStream, string coverFileExtension);

    public Task<(bool successfully, string error)> Delete(Guid userGuid, string sampleName);
}