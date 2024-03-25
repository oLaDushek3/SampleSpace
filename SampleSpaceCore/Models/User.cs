namespace SampleSpaceCore.Models;

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

    public Guid UserGuid { get; }

    public string Nickname { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public string? AvatarPath { get; private set; }

    public void ChangePassword(string newPassword) => Password = newPassword;

    public static (User User, string Error) Create(Guid userGuid, string nickname, string email, string password,
        string? avatarPath = null)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(nickname) || nickname.Length > MaxNicknameLength)
            error = "Nickname cannot be empty or longer then 75 symbols";

        if (string.IsNullOrEmpty(email))
            error = "Email cannot be empty";

        if (string.IsNullOrEmpty(password))
            error = "Email cannot be empty";

        var user = new User(userGuid, nickname, email, password, avatarPath);

        return (user, error);
    }

    // public (User user, string Error) Update(string nickname, string email, string password)
    // {
    //     var error = string.Empty;
    //
    //     if (string.IsNullOrEmpty(nickname) || nickname.Length > MaxNicknameLength)
    //         error = "Nickname cannot be empty or longer then 75 symbols";
    //
    //     if (string.IsNullOrEmpty(email))
    //         error = "Email cannot be empty";
    //
    //     if (string.IsNullOrEmpty(password))
    //         error = "Email cannot be empty";
    //
    //     Nickname = nickname;
    //     Email = email;
    //     Password = password;
    //     
    //     return (this, error);
    // }
}