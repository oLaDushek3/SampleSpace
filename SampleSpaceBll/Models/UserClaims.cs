namespace SampleSpaceBll.Models;

public class UserClaims
{
    public Guid UserGuid { get; set; }
    
    public bool IsAdmin { get; set; }
}