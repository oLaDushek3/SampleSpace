namespace SampleSpaceBll.Abstractions.Auth;

public interface IPasswordValidation
{
    public (bool valid, string error) Validation(string password);
}