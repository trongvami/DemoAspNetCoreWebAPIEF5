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
        [Authorize]
        public IActionResult Index()
        {
            //notyfService.Success("Hello world !");
            return View();
        }
    }
}
