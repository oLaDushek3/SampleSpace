namespace SampleSpaceCore.Models.Sample;

public class Sample
{
    private const int MaxNameLength = 75;
    private const int MaxArtistLength = 75;

    private Sample(Guid sampleGuid, string sampleLink, string coverLink, string name, string artist, Guid userGuid,
        double duration, string? vkontakteLink, string? spotifyLink, string? soundcloudLink, int numberOfListens, User? user, DateOnly date)
    {
        SampleGuid = sampleGuid;
        SampleLink = sampleLink;
        CoverLink = coverLink;
        Name = name;
        Artist = artist;
        UserGuid = userGuid;
        Duration = duration;
        VkontakteLink = vkontakteLink;
        SpotifyLink = spotifyLink;
        SoundcloudLink = soundcloudLink;
        NumberOfListens = numberOfListens;
        User = user;
        Date = date;
    }

    public Guid SampleGuid { get; private set; }

    public string SampleLink { get; private set; }

    public string CoverLink { get; private set; }

    public string Name { get; private set; }

    public string Artist { get; private set; }

    public Guid UserGuid { get; private set; }

    public double Duration { get; private set; }

    public string? VkontakteLink { get; private set; }

    public string? SpotifyLink { get; private set; }

    public string? SoundcloudLink { get; private set; }

    public int NumberOfListens { get; private set; }
    
    public User? User { get; private set; }
    
    public DateOnly Date { get; private set; }

    public static (Sample? Sample, string Error) Create(Guid sampleGuid, string sampleLink, string coverLink,
        string name, string artist, Guid userGuid, double duration, string? vkontakteLink,
        string? spotifyLink, string? soundcloudLink, int numberOfListens, User? user, DateOnly date)
    {
        if (string.IsNullOrEmpty(sampleLink))
            return (null, "Sample link cannot be empty");

        if (string.IsNullOrEmpty(coverLink))
            return (null, "Cover link cannot be empty");

        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
            return (null, $"Name cannot be empty or longer then {MaxNameLength} symbols");

        if (string.IsNullOrEmpty(artist) || artist.Length > MaxArtistLength)
            return (null, $"Artist cannot be empty or longer then {MaxArtistLength} symbols");

        var sample = new Sample(sampleGuid, sampleLink, coverLink, name, artist, userGuid, duration, vkontakteLink,
            spotifyLink, soundcloudLink, numberOfListens, user, date);

        return (sample, string.Empty);
    }
}