namespace SampleSpaceCore.Models.Sample;

public class CreatedSample
{
    private const int MaxNameLength = 75;
    private const int MaxArtistLength = 75;

    private CreatedSample(Guid sampleGuid, Stream sampleStream,
        double sampleStart, double sampleEnd, Stream coverStream,
        string name, string artist, Guid userGuid, string? vkontakteLink,
        string? spotifyLink, string? soundcloudLink)
    {
        SampleGuid = sampleGuid;
        SampleStream = sampleStream;
        SampleStart = sampleStart;
        SampleEnd = sampleEnd;
        CoverStream = coverStream;
        Name = name;
        Artist = artist;
        UserGuid = userGuid;
        VkontakteLink = vkontakteLink;
        SpotifyLink = spotifyLink;
        SoundcloudLink = soundcloudLink;
    }

    public Guid SampleGuid { get; private set; }

    public Stream SampleStream { get; private set; }

    public double SampleStart { get; private set; }

    public double SampleEnd { get; private set; }

    public Stream CoverStream { get; private set; }

    public string Name { get; private set; }

    public string Artist { get; private set; }

    public Guid UserGuid { get; private set; }

    public string? VkontakteLink { get; private set; }

    public string? SpotifyLink { get; private set; }

    public string? SoundcloudLink { get; private set; }

    public static (CreatedSample? CreatedSample, string Error) Create(Guid sampleGuid, Stream sampleStream,
        double sampleStart, double sampleEnd, Stream coverStream, string name, string artist, Guid userGuid,
        string? vkontakteLink, string? spotifyLink, string? soundcloudLink)
    {
        if (sampleStart > sampleEnd)
        {
            Dispose(sampleStream, coverStream);
            return (null, "Cut start cannot be greater than cut end");
        }

        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
        {
            Dispose(sampleStream, coverStream);
            return (null, $"Name cannot be empty or longer then {MaxNameLength} symbols");
        }

        if (string.IsNullOrEmpty(artist) || artist.Length > MaxArtistLength)
        {
            Dispose(sampleStream, coverStream);
            return (null, $"Artist cannot be empty or longer then {MaxArtistLength} symbols");
        }

        var createdSample = new CreatedSample(sampleGuid, sampleStream, sampleStart, sampleEnd, coverStream, name,
            artist, userGuid, vkontakteLink, spotifyLink, soundcloudLink);

        return (createdSample, string.Empty);
    }

    private static void Dispose(Stream sampleStream, Stream coverStream)
    {
        sampleStream.Dispose();
        coverStream.Dispose();
    }

    public void Dispose()
    {
        SampleStream.Dispose();
        CoverStream.Dispose();
    }
}