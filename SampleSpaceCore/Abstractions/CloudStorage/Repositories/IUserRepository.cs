namespace SampleSpaceCore.Abstractions.CloudStorage.Repositories;

public interface IUserRepository
{
    public Task<(string? avatarLink, string error)> Create(Guid userGuid, Stream avatarStream);

    public Task<(bool successfully, string error)> Delete(Guid userGuid);
}