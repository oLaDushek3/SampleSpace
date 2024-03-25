namespace SampleSpaceDal.Entities;

public class UserEntity
{
    public Guid UserGuid { get; set; }
    
    public string Nickname { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public string AvatarPath { get; set; } = string.Empty;
}