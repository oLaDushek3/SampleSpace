namespace SampleSpaceApi.Contracts.User;

public record UserRequest(
    string Nickname,
    string Email,
    string Password);