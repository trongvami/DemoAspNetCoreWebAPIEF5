using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        public INotyfService notyfService { get; }
        public AdminHomeController(INotyfService notyfService)
        {
            this.notyfService = notyfService;
        }
        
        [HttpGet]
        [Route("Admin/AdminHome/Index")]
        public IActionResult Index()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }
            return View();
        }
    }
}
