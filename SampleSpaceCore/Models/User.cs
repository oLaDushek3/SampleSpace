namespace SampleSpaceCore.Models;

public class User
{
    private const int MaxNicknameLength = 75;

    private User(Guid userGuid, string nickname, string email, string password, string? avatarPath, bool isAdmin)
    {
        UserGuid = userGuid;
        Nickname = nickname;
        Email = email;
        Password = password;
        AvatarPath = avatarPath;
        IsAdmin = isAdmin;
    }

    public Guid UserGuid { get; private set; }

    public string Nickname { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public string? AvatarPath { get; private set; }

    public bool IsAdmin { get; private set; }

    public void ChangePassword(string newPassword) => Password = newPassword;

    public void PutAvatarPath(string avatarPath) => AvatarPath = avatarPath;

    public static (User? User, string Error) Create(Guid userGuid, string nickname, string email, string? password,
        bool isAdmin = false, string? avatarPath = null)
    {
        if (string.IsNullOrEmpty(nickname) || nickname.Length > MaxNicknameLength)
            return (null, "Nickname cannot be empty or longer then 75 symbols");

        if (string.IsNullOrEmpty(email))
            return (null, "Email cannot be empty");

        if (string.IsNullOrEmpty(password))
            password = "";

        var user = new User(userGuid, nickname, email, password, avatarPath, isAdmin);

        return (user, string.Empty);
    }

    public (bool successful, string Error) Edit(string? nickname, string? email, string? avatarPath = null)
    {
        if (nickname != null)
        {
            if (nickname.Length > MaxNicknameLength)
                return (false, "Nickname longer then 75 symbols");

            Nickname = nickname;
        }

        if (email != null)
            Email = email;

        if (avatarPath != null)
            AvatarPath = avatarPath;

        return (true, string.Empty);
    }
}