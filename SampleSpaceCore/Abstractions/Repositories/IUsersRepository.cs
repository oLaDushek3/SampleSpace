using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.Repositories;

public interface IUsersRepository
{
    public Task<(User? user, string error)> GetByNickname(string nickname);

    public Task<(Guid? userGuid, string error)> Create(User user);
}