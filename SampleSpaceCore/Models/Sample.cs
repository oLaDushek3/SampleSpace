namespace SampleSpaceCore.Models;

public class Sample
{
    private const int MaxNameLength = 75;
    private const int MaxArtistLength = 75;
    
    private Sample(Guid sampleGuid, string samplePath, string coverPath, string name, string artist, Guid userGuid, int numberOfListens)
    {
        SampleGuid = sampleGuid;
        SamplePath = samplePath;
        CoverPath = coverPath;
        Name = name;
        Artist = artist;
        UserGuid = userGuid;
        NumberOfListens = numberOfListens;
    }
    
    public Guid SampleGuid { get; set; }

    public string SamplePath { get; private set; }
    
    public string CoverPath { get; private set; }

    public string Name { get; private set; }

    public string Artist { get; private set; }
    
    public Guid UserGuid { get; private set; }

    public int NumberOfListens { get; private set; } = 0;
    
    public static (Sample Sample, string Error) Create(Guid sampleGuid, string samplePath, string coverPath, string name, string artist, Guid userGuid, int numberOfListens)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(samplePath))
            error = "Sample path cannot be empty";

        if (string.IsNullOrEmpty(coverPath))
            error = "Cover path cannot be empty";
        
        if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
            error = $"Name cannot be empty or longer then {MaxNameLength} symbols";
        
        if (string.IsNullOrEmpty(artist) || artist.Length > MaxArtistLength)
            error = $"Artist cannot be empty or longer then {MaxArtistLength} symbols";

        var sample = new Sample(sampleGuid, samplePath, coverPath, name, artist, userGuid, numberOfListens);

        return (sample, error);
    }
}