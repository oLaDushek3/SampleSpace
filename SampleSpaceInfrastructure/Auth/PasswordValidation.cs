using System.Text.RegularExpressions;
using SampleSpaceBll.Abstractions.Auth;

namespace SampleSpaceInfrastructure.Auth;

public partial class PasswordValidation : IPasswordValidation
{
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex NumberRegex();
    
    [GeneratedRegex(@"[A-ZА-Я]+")]
    private static partial Regex UpperCharRegex();
    
    [GeneratedRegex(@".{5,}")]
    private static partial Regex Minimum5CharsRegex();
    
    public (bool valid, string error) Validation(string password)
    {
        var hasNumber = NumberRegex();
        var hasUpperChar = UpperCharRegex();
        var hasMinimum5Chars = Minimum5CharsRegex();

        if(!hasNumber.IsMatch(password))
            return (false, "Password must contain numbers");
                
        if(!hasUpperChar.IsMatch(password))
            return (false, "Password must contain uppercase chars");
                
        if(!hasMinimum5Chars.IsMatch(password))
            return (false, "Password must contain minimum 5 chars");

        return (true, string.Empty);
    }
}