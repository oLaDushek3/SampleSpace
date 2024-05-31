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
        IPasswordValidation passwordValidation, ITokenManager tokenManager, IEmailService emailService)
    : IUserService
{
    public async Task<(string? avatarLink, string error)> UploadUserAvatar(Guid userGuid, Stream avatarStream)
    {
        var (avatarLink, error) = await cloudStorageUserRepository.Create(userGuid, avatarStream);

        return !string.IsNullOrEmpty(error) ? (null, error) : (avatarLink, string.Empty);
    }

    public (bool successfully, string error) PasswordValidation(string password)
    {
        var (valid, validError) = passwordValidation.Validation(password);

        if (!valid)
            return (false, validError);
        
        return (true, string.Empty);
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

    public async Task<(User? user, string error)> ForgotPassword(string email, string route)
    {
        var (user, error) = await postgreSqlUserRepository.GetByEmail(email);

        if (!string.IsNullOrEmpty(error))
            return (null, error);

        if (user == null)
            return (null, string.Empty);

        string resetToken = tokenManager.CreateResetToken(user.UserGuid);

        await SendResetEmail(email, resetToken, route);

        return (user, string.Empty);
    }

    public async Task<(bool successfully, string error, int errorCode)> ResetPassword(string resetToken,
        string newPassword) 
    {
        Guid userGuid;
        try
        {
            userGuid = tokenManager.GetUserIdFromToken(resetToken);
            ;
        }
        catch
        {
            return (false, "Invalid token", 403);
        }

        var (user, getError) = await postgreSqlUserRepository.GetByGuid(userGuid: userGuid);

        if (!string.IsNullOrEmpty(getError))
            return (false, getError, 404);

        var resetTokenValid = tokenManager.CheckTokenValid(resetToken);

        if (user == null || !resetTokenValid)
            return (false, "Invalid token", 403);

        var resetTokenActive = tokenManager.CheckTokenActive(resetToken);

        if (!resetTokenActive)
            return (false, "Token expired", 403);

        user.ChangePassword(passwordHasher.Generate(newPassword));

        var (successfully, saveError) = await postgreSqlUserRepository.Edit(user);

        if (!string.IsNullOrEmpty(saveError))
            return (false, saveError, 400);

        return (true, string.Empty, 200);
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

    public async Task<(User? loginUser, string error)> GetUserByEmail(string email)
    {
        var (user, error) = await postgreSqlUserRepository.GetByEmail(email);

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