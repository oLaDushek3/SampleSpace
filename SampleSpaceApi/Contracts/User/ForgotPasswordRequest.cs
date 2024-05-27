namespace SampleSpaceApi.Contracts.User;

public record ForgotPasswordRequest(
    string Route,
    string Email);