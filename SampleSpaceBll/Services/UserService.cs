using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceBll.Services;

public class UserService(IUsersRepository userRepository, IPasswordHasher passwordHasher, ITokenManager tokenManager, ICookieManager cookieManager)
    : IUserService
{
    public async Task<(Guid? userGuid, string error)> SignUp(User newUser)
    {
        var hashedPassword = passwordHasher.Generate(newUser.Password);

        newUser.ChangePassword(hashedPassword);

        var(userGuid, error) = await userRepository.Create(newUser);
        
        return !string.IsNullOrEmpty(error) ? (null, error) : (userGuid, string.Empty);
    }
    
    public async Task<(User? loginUser, string error)> SignIn(HttpResponse response, string nickname, string password)
    {
        var (user, error) = await userRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, error);
        
        if (user == null)
            return (null, "Failed to login");

        var passwordVerifyResult = passwordHasher.Verify(password, user.Password);
        
        if (!passwordVerifyResult)
            return (null, "Failed to login");

        var tokens = tokenManager.CreateTokens(user.UserGuid);

        try
        {
            await tokenManager.SaveTokens(response, tokens, user.UserGuid);
        }
        catch (Exception e)
        {
            return (user, e.Message);
        }
        
        return (user, string.Empty);
    }
    
    public async Task<(bool successfully, string error)> SignOut(HttpContext context)
    {
        await tokenManager.DeleteTokens(context);
        return (true, string.Empty);
    }

    public async Task<(User? loginUser, string error)> GetUser(string nickname)
    {
        var (user, error) = await userRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        return user == null ? (null, "User not found") : (user, string.Empty);
    }
}