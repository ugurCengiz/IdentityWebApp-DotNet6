using IdentityUyelikSistemi_DotNet6.Enums;
using IdentityUyelikSistemi_DotNet6.Models;
using IdentityUyelikSistemi_DotNet6.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityUyelikSistemi_DotNet6.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        private AppUser user = new AppUser();
       
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager):base(userManager, signInManager)
        {
           
        }

        public IActionResult Index()
        {
            user = CurrentUser;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();


            return View(userViewModel);
        }

        public IActionResult UserEdit()
        {
            user = CurrentUser;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel,IFormFile userPicture)
        {
            ModelState.Remove("Password");

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            
                user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (userPicture != null && userPicture.Length>0)
                {
                    var fileName = new Guid().ToString() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);

                        user.Picture = "/UserPicture/" + fileName;
                    }

                }
                
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;

                

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);

                    ViewBag.success = "true";

                }
                else
                {
                    AddModelError(result);
                }
            

            return View(userViewModel);
        }

        public void LogOut()
        {
            _signInManager.SignOutAsync();
           
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                user = CurrentUser;


                bool exits = _userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;

                if (exits)
                {
                    IdentityResult result = _userManager.ChangePasswordAsync(user,
                        passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;
                    if (result.Succeeded)
                    {
                        _userManager.UpdateSecurityStampAsync(user);
                        _signInManager.SignOutAsync();
                        _signInManager.PasswordSignInAsync(user.UserName, passwordChangeViewModel.PasswordNew, true,
                            false);

                        ViewBag.success = "true";
                    }
                    else
                    {
                        AddModelError(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }

            }

            return View(passwordChangeViewModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Manager()
        {
            return View();
        }
    }
}
