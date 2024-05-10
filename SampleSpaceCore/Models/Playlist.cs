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

    public Guid PlaylistGuid { get; private set; }

    public Guid UserGuid { get; private set; }

    public string Name { get; private set; }

    public static (Playlist? playlist, string Error) Create(Guid playlistGuid, Guid userGuid, string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
            return (null, "Nickname cannot be empty or longer then 75 symbols");

        var playlist = new Playlist(playlistGuid, userGuid, name);

        return (playlist, string.Empty);
    }
    
    public (Playlist? playlist, string error) Edit(string newName)
    {
        if(string.IsNullOrEmpty(newName) || newName.Length > MaxNameLength)
            return (null, $"Name cannot be empty or longer then {MaxNameLength} symbols");

        Name = newName;
        
        return (this, string.Empty);
    }
}