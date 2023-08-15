using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
