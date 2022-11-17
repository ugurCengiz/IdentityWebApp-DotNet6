using System.ComponentModel.DataAnnotations;

namespace IdentityUyelikSistemi_DotNet6.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru formatta değil.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Şifre gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
