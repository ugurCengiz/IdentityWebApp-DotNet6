using IdentityUyelikSistemi_DotNet6.Models;
using IdentityUyelikSistemi_DotNet6.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUyelikSistemi_DotNet6.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        AppRole role = new AppRole();
        private AppUser user = new AppUser();

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {

        }


        public IActionResult Index()
        {
            return View();

        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {


            role.Name = roleViewModel.Name;

            IdentityResult result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);
            }

            return View(roleViewModel);
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

            }
            return RedirectToAction("Roles");
        }

        public IActionResult RoleUpdate(string id)
        {
            role = _roleManager.FindByIdAsync(id).Result;

            return View(role.Adapt<RoleViewModel>());
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız");
            }

            return View(roleViewModel);
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            user = _userManager.FindByIdAsync(id).Result;
            ViewBag.userName = user.UserName;

            IQueryable<AppRole> roles = _roleManager.Roles;

            List<string> userRoles = _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel roleAssign = new RoleAssignViewModel();
                roleAssign.RoleId = role.Id;
                roleAssign.RoleName = role.Name;
                if (userRoles.Contains(role.Name))
                {
                    
                    roleAssign.Exist = true;

                }
                else 
                {
                    
                    roleAssign.Exist = false;
                }

                roleAssignViewModels.Add(roleAssign);
            }


            return View(roleAssignViewModels);
        }

        [HttpPost]

        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)
                {
                  await  _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                   await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }


            return RedirectToAction("Users");
        }


    }
}
