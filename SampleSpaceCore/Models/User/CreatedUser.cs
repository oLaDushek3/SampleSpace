namespace SampleSpaceCore.Models.User;

public class CreatedUser
{
    private const int MaxNicknameLength = 75;

    private CreatedUser(Guid userGuid, string nickname, string email, string password, Stream? avatarStream)
    {
        UserGuid = userGuid;
        Nickname = nickname;
        Email = email;
        Password = password;
        AvatarStream = avatarStream;
    }

    public Guid UserGuid { get; private set; }

    public string Nickname { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public Stream? AvatarStream { get; private set; }

    public static (CreatedUser? createdUser, string error) Create(Guid userGuid, string nickname, string email,
        string? password, Stream? avatarStream = null)
    {
        if (string.IsNullOrEmpty(nickname) || nickname.Length > MaxNicknameLength)
        {
            Dispose(avatarStream);
            return (null, "Nickname cannot be empty or longer then 75 symbols");
        }

        if (string.IsNullOrEmpty(email))
        {
            Dispose(avatarStream);
            return (null, "Email cannot be empty");
        }

        if (string.IsNullOrEmpty(password))
            password = "";

        var createdUser = new CreatedUser(userGuid, nickname, email, password, avatarStream);

        return (createdUser, string.Empty);
    }

    private static void Dispose(Stream? avatarStream)
    {
        avatarStream?.Dispose();
    }

    public void Dispose()
    {
        AvatarStream?.Dispose();
    }
}