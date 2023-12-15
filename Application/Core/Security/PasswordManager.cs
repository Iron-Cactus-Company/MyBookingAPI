using Domain;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Security;

public class PasswordManager(IPasswordHasher<BusinessProfile> passwordHasher)
{
    public string HashPassword(string password)
    {
        string hashedPassword = passwordHasher.HashPassword(null, password);
        return hashedPassword;
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}