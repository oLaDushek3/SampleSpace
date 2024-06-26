using SampleSpaceCore.Models;

namespace SampleSpaceCore.Abstractions.PostgreSQL.Repositories;

public interface IUsersRepository
{
    public Task<(User? user, string error)> GetByGuid(Guid userGuid);
    
    public Task<(User? user, string error)> GetByNickname(string nickname);
    
    public Task<(User? user, string error)> GetByEmail(string email);

    public Task<(Guid? userGuid, string error)> Create(User user);
    
    public Task<(bool successfully, string error)> Edit(User user);
    
    public Task<(bool successfully, string error)> Delete(Guid userGuid);
}