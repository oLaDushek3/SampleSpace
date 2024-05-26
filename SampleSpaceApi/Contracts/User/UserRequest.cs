namespace SampleSpaceApi.Contracts.User;

public record UserRequest(
    IFormFile? AvatarFile,
    string Nickname,
    string Email,
    string Password);