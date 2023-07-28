using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class HomeController : Controller
    {
        private readonly IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            //var userName = await _cache.GetStringAsync("UserName");
            //string userValue = HttpContext.Session.GetString("UserName");

            //if (userName != null || userValue != null)
            //{
            //    ViewData["UserName"] = userValue == null ? userName : userValue;
            //    return View();
            //}
            //else { 
            //    return RedirectToAction("Login", "Account", new { area = "Admin" });
            //}

            return View();

        }
    }
}
