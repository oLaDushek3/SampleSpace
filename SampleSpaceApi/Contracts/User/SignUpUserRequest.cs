namespace SampleSpaceApi.Contracts.User;

public record SignUpUserRequest(
    IFormFile? AvatarFile,
    string Nickname,
    string Email,
    string Password);