namespace SampleSpaceApi.Contracts.User;

public record UserResponse(
    Guid UserGuid,
    string? AvatarPath,
    string Nickname,
    string Email);