using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions;

public interface IUsersRepository
{
    public Task<User?> GetByNickname(string email);

    public Task<Guid> Create(User user);
}