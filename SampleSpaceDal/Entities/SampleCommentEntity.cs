namespace SampleSpaceDal.Entities;

public class SampleCommentEntity
{
    public Guid SampleCommentGuid { get; set; }
    
    public Guid SampleGuid { get; set; }
    
    public Guid UserGuid { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Comment { get; set; } = string.Empty;
    
    public UserEntity User { get; set; } = new();
}