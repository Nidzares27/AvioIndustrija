using AvioIndustrija.Models;
using Microsoft.AspNetCore.Identity;

namespace AvioIndustrija
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (password.Length < 6)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Password must be at least 6 characters long."
                });
            }
            if (!password.Any(char.IsDigit))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresDigit",
                    Description = "Password must contain at least one digit."
                });
            }
            if (!password.Any(char.IsLower))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresLowercase",
                    Description = "Password must contain at least one lowercase letter."
                });
            }
            if (!password.Any(char.IsUpper))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresUppercase",
                    Description = "Password must contain at least one uppercase letter."
                });
            }
            if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric",
                    Description = "Password must contain at least one non-alphanumeric character."
                });
            }


            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}
