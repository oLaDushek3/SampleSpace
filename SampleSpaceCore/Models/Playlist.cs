namespace SampleSpaceCore.Models;

public class Playlist
{
    private const int MaxNameLength = 75;

    private Playlist(Guid playlistGuid, Guid userGuid, string name)
    {
        PlaylistGuid = playlistGuid;
        UserGuid = userGuid;
        Name = name;
    }

    public Guid PlaylistGuid { get; set; }

    public Guid UserGuid { get; set; }

    public string Name { get; set; }

    public static (Playlist? Playlist, string Error) Create(Guid playlistGuid, Guid userGuid, string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
            return (null, "Nickname cannot be empty or longer then 75 symbols");

        var playlist = new Playlist(playlistGuid, userGuid, name);

        return (playlist, string.Empty);
    }
}