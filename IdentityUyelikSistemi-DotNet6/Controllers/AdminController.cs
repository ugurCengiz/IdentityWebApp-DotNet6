using IdentityUyelikSistemi_DotNet6.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUyelikSistemi_DotNet6.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            
            return View(_userManager.Users.ToList());
        }
    }
}
