using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using ServerSide.Models.ViewModels.Harmichome;
using System.Net.Http.Headers;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SaleClient;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IHttpClientFactory httpClientFactory, IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }

        [Route("/product/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Index(int id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetProductByIdUrl = "https://localhost:7071/api/Sale/GetProductDetailById/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response5 = JsonConvert.DeserializeObject<ViewModels.SaleClient.ProductDetailVM>(httpClient.GetStringAsync(apiGetProductByIdUrl + id).Result);
            return View(response5);
        }
    }
}
