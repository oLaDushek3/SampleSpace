namespace SampleSpaceCore.Models.User;

public class User
{
    private const int MaxNicknameLength = 75;

    private User(Guid userGuid, string nickname, string email, string password, string? avatarPath)
    {
        UserGuid = userGuid;
        Nickname = nickname;
        Email = email;
        Password = password;
        AvatarPath = avatarPath;
    }

    public Guid UserGuid { get; private set; }

    public string Nickname { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public string? AvatarPath { get; private set; }

    public void ChangePassword(string newPassword) => Password = newPassword;

    public void PutAvatarPath(string avatarPath) => AvatarPath = avatarPath;
    
    public static (User? User, string Error) Create(Guid userGuid, string nickname, string email, string? password,
        string? avatarPath = null)
    {
        if (string.IsNullOrEmpty(nickname) || nickname.Length > MaxNicknameLength)
            return (null, "Nickname cannot be empty or longer then 75 symbols");

        if (string.IsNullOrEmpty(email))
            return (null, "Email cannot be empty");

        if (string.IsNullOrEmpty(password))
            password = "";

        var user = new User(userGuid, nickname, email, password, avatarPath);

        return (user, string.Empty);
    }
}