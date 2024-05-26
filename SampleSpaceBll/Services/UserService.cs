using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models.User;
using IPostgreSQLUserRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.IUsersRepository;
using ICloudStorageUserRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.IUserRepository;

namespace SampleSpaceBll.Services;

public class UserService(IPostgreSQLUserRepository postgreSqlUserRepository,
        ICloudStorageUserRepository cloudStorageUserRepository, IPasswordHasher passwordHasher,
        ITokenManager tokenManager)
    : IUserService
{
    public async Task<(string? avatarLink, string error)> UploadUserAvatar(Guid userGuid, Stream avatarStream)
    {
        var (avatarLink, error) = await cloudStorageUserRepository.Create(userGuid, avatarStream);

        return !string.IsNullOrEmpty(error) ? (null, error) : (avatarLink, string.Empty);
    }

    public async Task<(Guid? userGuid, string error)> SignUp(User newUser)
    {
        var hashedPassword = passwordHasher.Generate(newUser.Password);

        newUser.ChangePassword(hashedPassword);

        var (userGuid, error) = await postgreSqlUserRepository.Create(newUser);

        return !string.IsNullOrEmpty(error) ? (null, error) : (userGuid, string.Empty);
    }

    public async Task<(User? loginUser, string error)> SignIn(HttpResponse response, string nickname, string password)
    {
        var (user, error) = await postgreSqlUserRepository.GetByNickname(nickname);

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
        var (user, error) = await postgreSqlUserRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        return user == null ? (null, "User not found") : (user, string.Empty);
    }
}