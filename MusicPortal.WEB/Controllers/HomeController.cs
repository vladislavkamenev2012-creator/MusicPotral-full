using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Songs");
    }
}
