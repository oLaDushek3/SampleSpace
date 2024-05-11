namespace SampleSpaceDal.Entities;

public class PlaylistEntity
{
    public Guid PlaylistGuid { get; set; }

    public Guid UserGuid { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool CanBeModified { get; set; } = true;
}