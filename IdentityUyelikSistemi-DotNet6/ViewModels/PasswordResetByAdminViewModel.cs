using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IdentityUyelikSistemi_DotNet6.ViewModels
{
    public class PasswordResetByAdminViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Yeni şifre")]
        public string NewPassword { get; set; }

    }
}
