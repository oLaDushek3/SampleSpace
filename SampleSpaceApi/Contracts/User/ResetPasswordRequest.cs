namespace SampleSpaceApi.Contracts.User;

public record ResetPasswordRequest(
    string ResetToken,
    string Password);