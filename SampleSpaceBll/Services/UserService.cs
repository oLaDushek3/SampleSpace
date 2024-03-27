using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class UserService(IUsersRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    : IUserService
{
    public async Task<Guid> SigUp(User newUser)
    {
        var hashedPassword = passwordHasher.Generate(newUser.Password);

        newUser.ChangePassword(hashedPassword);

        return await userRepository.Create(newUser);
    }
    
    public async Task<(User? loginUser, string error)> GetUser(string nickname)
    {
        var error = string.Empty;
        
        var user = await userRepository.GetByNickname(nickname);

        if (user != null) return (user, error);
        
        error = "User was not found";
        return (null, error);
    }

    public async Task<(User? loginUser, string? token, string error)> SigIn(string nickname, string password)
    {
        var error = string.Empty;
        
        var user = await userRepository.GetByNickname(nickname);

        if (user == null)
        {
            error = "Failed to login";
            return (null, null, error);
        }

        var result = passwordHasher.Verify(password, user.Password);
        
        if (!result)
        {
            error = "Failed to login";
            return (null, null, error);
        }

        var token = jwtProvider.GenerateToken(user);
        
        return (user, token, error);
    }
}