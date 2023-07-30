using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class AdminHomeController : Controller
    {
        [HttpGet]
        [Route("Admin/AdminHome/Index")]
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
