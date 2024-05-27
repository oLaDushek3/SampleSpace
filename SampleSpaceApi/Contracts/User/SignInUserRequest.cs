namespace SampleSpaceApi.Contracts.User;

public record SignInUserRequest(
    string Nickname,
    string Password);