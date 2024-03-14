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

    public async Task<(string? token, string error)> SigIn(string nickname, string password)
    {
        var error = string.Empty;
        
        var userEntity = await userRepository.GetByNickname(nickname);

        if (userEntity == null)
        {
            error = "Failed to login";
            return (null, error);
        }

        var result = passwordHasher.Verify(password, userEntity.Password);
        
        if (!result)
        {
            error = "Failed to login";
            return (null, error);
        }

        var token = jwtProvider.GenerateToken(userEntity);
        
        return (token, error);
    }
}