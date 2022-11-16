using Microsoft.AspNetCore.Mvc;

namespace IdentityUyelikSistemi_DotNet6.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
