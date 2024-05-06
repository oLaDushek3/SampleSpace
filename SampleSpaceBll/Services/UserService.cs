using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class UserService(IUsersRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    : IUserService
{
    public async Task<(Guid? userGuid, string error)> SigUp(User newUser)
    {
        var hashedPassword = passwordHasher.Generate(newUser.Password);

        newUser.ChangePassword(hashedPassword);

        var(userGuid, error) = await userRepository.Create(newUser);
        
        return !string.IsNullOrEmpty(error) ? (null, error) : (userGuid, string.Empty);
    }
    
    public async Task<(User? loginUser, string error)> GetUser(string nickname)
    {
        var (user, error) = await userRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        return user == null ? (null, "User not found") : (user, string.Empty);
    }

    public async Task<(User? loginUser, string? token, string error)> SigIn(string nickname, string password)
    {
        var (user, error) = await userRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, null, error);
        
        if (user == null)
            return (null, null, "Failed to login");

        var passwordVerifyResult = passwordHasher.Verify(password, user.Password);
        
        if (!passwordVerifyResult)
            return (null, null, "Failed to login");

        var token = jwtProvider.GenerateToken(user);
        
        return (user, token, error);
    }
}