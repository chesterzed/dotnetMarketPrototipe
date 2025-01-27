using Microsoft.AspNetCore.Mvc;

namespace MyFirstProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
