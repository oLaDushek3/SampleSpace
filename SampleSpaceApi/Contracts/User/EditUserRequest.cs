namespace SampleSpaceApi.Contracts.User;

public record EditUserRequest(
    Guid UserGuid,
    IFormFile? AvatarFile,
    string? Nickname,
    string? Email);