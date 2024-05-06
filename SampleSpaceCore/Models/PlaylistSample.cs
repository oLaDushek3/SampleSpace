namespace SampleSpaceCore.Models;

public class PlaylistSample
{
    private PlaylistSample(Guid playlistSampleEntityGuid, Guid playlistGuid, Guid sampleGuid)
    {
        PlaylistSampleEntityGuid = playlistSampleEntityGuid;
        PlaylistGuid = playlistGuid;
        SampleGuid = sampleGuid;
    }

    public Guid PlaylistSampleEntityGuid { get; private set; }
    
    public Guid PlaylistGuid { get; private set; }
    
    public Guid SampleGuid { get; private set; }

    public static (PlaylistSample PlaylistSample, string Error) Create(Guid playlistSampleEntityGuid, Guid playlistGuid, Guid sampleGuid)
    {
        var playlistSample = new PlaylistSample(playlistSampleEntityGuid, playlistGuid, sampleGuid);

        return (playlistSample, string.Empty);
    }
}