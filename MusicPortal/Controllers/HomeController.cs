using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
