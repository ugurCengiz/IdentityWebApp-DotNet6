﻿using IdentityUyelikSistemi_DotNet6.Models;
using IdentityUyelikSistemi_DotNet6.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityUyelikSistemi_DotNet6.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppUser> _userManager;
        public SignInManager<AppUser> _signInManager;

        AppUser user = new AppUser();

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByEmailAsync(userLogin.Email);
                if (user != null)
                {
                    if (await  _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("","Hesabınız Bir Süreliğine Kitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        return View(userLogin);
                    }

                    await _signInManager.SignOutAsync();

                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, false);

                    if (signInResult.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"]!=null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                        
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        

                        int fail = await _userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("",$"{fail} kez başarısız giriş.");

                        if (fail==3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user,new DateTimeOffset(DateTime.Now.AddMinutes(20)));

                            ModelState.AddModelError("","Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email adresiniz veya şifresi yanlış.");
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");

                }
            }

            return View(userLogin);
        }


        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(userViewModel);
        }



    }
}
