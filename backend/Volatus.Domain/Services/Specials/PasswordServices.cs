using System.Security.Cryptography;
using System.Text;

using Volatus.Domain.Exceptions;
using Volatus.Domain.Interfaces.Services.Specials;

namespace Volatus.Domain.Services.Specials;

public class PasswordServices : IPasswordServices
{
    public string Hash(string password)
    {
        ThrowIfPasswordIsInvalid(password);
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool Check(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    
    private static void ThrowIfPasswordIsInvalid(string password)
    {
        var errors = new List<string>();

        if (password.Length < 8)
            errors.Add("Password must be at least 8 characters long.");

        if (!password.Any(char.IsUpper))
            errors.Add("Password must contain at least one uppercase letter.");

        if (!password.Any(char.IsLower))
            errors.Add("Password must contain at least one lowercase letter.");

        if (!password.Any(char.IsDigit))
            errors.Add("Password must contain at least one number.");

        if (!password.Any(char.IsSymbol))
            errors.Add("Password must contain at least one symbol.");

        if (errors.Any())
            throw new BadRequestException(string.Join(", ", errors));
    }
}
