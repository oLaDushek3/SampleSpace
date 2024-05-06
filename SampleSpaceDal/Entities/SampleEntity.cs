namespace SampleSpaceDal.Entities;

public class SampleEntity
{
    public Guid SampleGuid { get; set; }

    public string SampleLink { get; set; } = string.Empty;
    
    public string CoverLink { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;
    
    public Guid UserGuid { get; set; }
    
    public double Duration { get; set; }
    
    public string VkontakteLink { get; set; } = string.Empty;
    
    public string SpotifyLink { get; set; } = string.Empty;
    
    public string SoundcloudLink { get; set; } = string.Empty;
    
    public int NumberOfListens { get; set; }

    public UserEntity User { get; set; } = new UserEntity();
}