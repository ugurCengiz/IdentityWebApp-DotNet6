using IdentityUyelikSistemi_DotNet6.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUyelikSistemi_DotNet6.CustomValidation
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        private List<IdentityError> errors = new List<IdentityError>();
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            string[] Digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            foreach (var item in Digits)
            {
                if (user.UserName[0].ToString() == item)
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "UserNameContainsFirstLetterDigitContains",
                        Description = "Kullanıcı adının ilk karakteri sayısal karakter içeremez."
                    });

                }
            }

            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

        }
    }
}
