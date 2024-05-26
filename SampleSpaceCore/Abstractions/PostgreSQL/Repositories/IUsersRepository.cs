using SampleSpaceCore.Models;
using SampleSpaceCore.Models.User;

namespace SampleSpaceCore.Abstractions.PostgreSQL.Repositories;

public interface IUsersRepository
{
    public Task<(User? user, string error)> GetByNickname(string nickname);

    public Task<(Guid? userGuid, string error)> Create(User user);
}