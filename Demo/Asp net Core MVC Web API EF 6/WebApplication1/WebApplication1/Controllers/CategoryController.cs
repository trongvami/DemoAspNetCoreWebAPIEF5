using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
