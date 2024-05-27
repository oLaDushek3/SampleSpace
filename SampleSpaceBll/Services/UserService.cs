using Microsoft.AspNetCore.Http;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Abstractions.AuthScheme;
using SampleSpaceBll.Abstractions.Email;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;
using IPostgreSQLUserRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.IUsersRepository;
using ICloudStorageUserRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.IUserRepository;

namespace SampleSpaceBll.Services;

public class UserService(IPostgreSQLUserRepository postgreSqlUserRepository,
        ICloudStorageUserRepository cloudStorageUserRepository, IPasswordHasher passwordHasher,
        ITokenManager tokenManager, IEmailService emailService)
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

    public async Task<(bool successfully, string error)> ForgotPassword(string email, string route)
    {
        var (user, error) = await postgreSqlUserRepository.GetByEmail(email);

        if (!string.IsNullOrEmpty(error))
            return (false, error);

        if (user == null)
            return (false, "User not found");

        string resetToken = tokenManager.CreateResetToken(user.UserGuid);

        await SendResetEmail(email, resetToken, route);

        return (true, string.Empty);
    }

    public async Task<(bool successfully, string error)> ResetPassword(string resetToken, string newPassword)
    {
        var userGuid = tokenManager.GetUserIdFromToken(resetToken);

        var (user, error) = await postgreSqlUserRepository.GetByGuid(userGuid);

        if (!string.IsNullOrEmpty(error))
            return (false, error);

        var resetTokenValid = tokenManager.CheckTokenValid(resetToken);

        if (user == null || !resetTokenValid)
            return (false, "Invalid token");

        var resetTokenActive = tokenManager.CheckTokenActive(resetToken);

        if (!resetTokenActive)
            return (false, "Token expired");
        
        user.ChangePassword(passwordHasher.Generate(newPassword));

        return await postgreSqlUserRepository.Edit(user);
    }

    public async Task<(bool successfully, string error)> SignOut(HttpContext context)
    {
        await tokenManager.DeleteTokens(context);
        return (true, string.Empty);
    }

    public async Task<(User? loginUser, string error)> GetUserByNickname(string nickname)
    {
        var (user, error) = await postgreSqlUserRepository.GetByNickname(nickname);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        return user == null ? (null, "User not found") : (user, string.Empty);
    }

    public async Task<(User? loginUser, string error)> GetUserByGuid(Guid userGuid)
    {
        var (user, error) = await postgreSqlUserRepository.GetByGuid(userGuid);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        return user == null ? (null, "User not found") : (user, string.Empty);
    }

    public async Task<(bool successfully, string error)> EditUser(User user)
    {
        return await postgreSqlUserRepository.Edit(user);
    }
    
    public async Task<(bool successfully, string error)> Delete(User user)
    {
        return await postgreSqlUserRepository.Delete(user.UserGuid);
    }

    private async Task SendResetEmail(string email, string resetToken, string route)
    {
        var resetUrl = $"{route}?token={resetToken}";
        string message = $@"<p>Пожалуйста, перейдите по ссылке ниже, чтобы сбросить свой пароль, 
                            ссылка будет действительна в течение 1 дня:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

        await emailService.Send(
            to: email, 
            subject: "Сброс пароля", 
            html: $@"<h4>Сброс Пароля</h4>
                    {message}");
    }
}