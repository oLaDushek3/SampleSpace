namespace SampleSpaceCore.Models;

public class PlaylistRelativeSample
{
    private PlaylistRelativeSample(Guid playlistGuid, bool contain, Playlist playlist)
    {
        PlaylistGuid = playlistGuid;
        Contain = contain;
        Playlist = playlist;
    }

    public Guid PlaylistGuid { get; private set; }

    public bool Contain { get; private set; }
    
    public Playlist Playlist { get; private set; }

    public static (PlaylistRelativeSample? playlist, string Error) Create(Playlist playlist, bool contain)
    {
        var playlistRelativeSample = new PlaylistRelativeSample(playlist.PlaylistGuid, contain, playlist);

        return (playlistRelativeSample, string.Empty);
    }
}