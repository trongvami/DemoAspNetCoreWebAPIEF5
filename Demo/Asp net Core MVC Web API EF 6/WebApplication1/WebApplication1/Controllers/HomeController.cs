using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NToastNotify;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SaleClient;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetProductsByCateAreaUrl = "https://localhost:7071/api/Sale/GetProductsByCateArea";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<ProductHomeVM>>(httpClient.GetStringAsync(apiGetProductsByCateAreaUrl).Result);
            HomeViewVM homeViewVM = new HomeViewVM
            {
                Products = response
            };
            return View(homeViewVM);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}