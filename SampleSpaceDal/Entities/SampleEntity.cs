namespace SampleSpaceDal.Entities;

public class SampleEntity
{
    public Guid SampleGuid { get; set; }

    public string SamplePath { get; set; } = string.Empty;
    
    public string CoverPath { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;
    
    public Guid UserGuid { get; set; }
    
    public int NumberOfListens { get; set; }
    
    public double Duration { get; set; }
}