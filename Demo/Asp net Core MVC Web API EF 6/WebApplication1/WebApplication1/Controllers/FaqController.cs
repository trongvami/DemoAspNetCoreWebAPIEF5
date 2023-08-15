using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
