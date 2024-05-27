namespace SampleSpaceCore.Models;

public class Playlist
{
    private const int MaxNameLength = 75;

    private Playlist(Guid playlistGuid, Guid userGuid, string name, bool canBeModified)
    {
        PlaylistGuid = playlistGuid;
        UserGuid = userGuid;
        Name = name;
        CanBeModified = canBeModified;
    }

    public Guid PlaylistGuid { get; private set; }

    public Guid UserGuid { get; private set; }

    public string Name { get; private set; }

    public bool CanBeModified { get; private set; }

    public static (Playlist? playlist, string error) Create(Guid playlistGuid, Guid userGuid, string name, bool canBeModified = true)
    {
        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
            return (null, "Nickname cannot be empty or longer then 75 symbols");

        var playlist = new Playlist(playlistGuid, userGuid, name, canBeModified);

        return (playlist, string.Empty);
    }
    
    public (bool successful, string error) Edit(string newName)
    {
        if(string.IsNullOrEmpty(newName) || newName.Length > MaxNameLength)
            return (false, $"Name cannot be empty or longer then {MaxNameLength} symbols");

        Name = newName;
        
        return (true, string.Empty);
    }
}