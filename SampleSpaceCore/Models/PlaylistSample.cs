namespace SampleSpaceCore.Models;

public class PlaylistSample
{
    private PlaylistSample(Guid playlistSampleGuid, Guid playlistGuid, Guid sampleGuid)
    {
        PlaylistSampleGuid = playlistSampleGuid;
        PlaylistGuid = playlistGuid;
        SampleGuid = sampleGuid;
    }

    public Guid PlaylistSampleGuid { get; private set; }
    
    public Guid PlaylistGuid { get; private set; }
    
    public Guid SampleGuid { get; private set; }

    public static (PlaylistSample PlaylistSample, string Error) Create(Guid playlistSampleEntityGuid, Guid playlistGuid, Guid sampleGuid)
    {
        var playlistSample = new PlaylistSample(playlistSampleEntityGuid, playlistGuid, sampleGuid);

        return (playlistSample, string.Empty);
    }
}