namespace SampleSpaceApi.Contracts.User;

public record LoginUserRequest(
    string Nickname,
    string Password);