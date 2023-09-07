using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModels;
using ServerSide.Models.ViewModels.Harmichome;
using WebApplication1.ViewModels.SaleClient;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class ShopController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;
        private readonly ILogger<ProductController> _logger;

        public ShopController(ILogger<ProductController> logger, IHttpClientFactory httpClientFactory, IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }

        [Route("Shop/GetLevels")]
        [HttpGet]
        public async Task<IActionResult> GetLevels(string? selectedParent)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient.GetStringAsync(apiGetAllLevelUrl).Result);
            var queryableResponse = response.AsQueryable();
            if (!string.IsNullOrWhiteSpace(selectedParent))
            {
                if (selectedParent == "0")
                {
                    List<SelectListItem> levels = response.Select(
                    n => new SelectListItem
                    {
                        Value = n.LevelCode,
                        Text = n.LevelName
                    }).ToList();
                    return Json(levels);
                }
                else
                {
                    List<SelectListItem> levels = response.Where(c => c.ParentID.ToString() == selectedParent).Select(
                    n => new SelectListItem
                    {
                        Value = n.LevelCode,
                        Text = n.LevelName
                    }).ToList();
                    return Json(levels);
                }
            }

            return null;
        }

        [Route("shop.html", Name = "ShopProduct")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page, int? selectedParent, int? selectedLevel, string? searchProduct, string? priceF, string? priceT, string? orderby)
        {
            try
            {
                int LevelCode = (int)(selectedLevel != null ? selectedLevel : 0);
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 9;

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                if (selectedParent == null)
                {
                    selectedParent = 0;
                }

                var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
                var httpClient2 = _httpClientFactory.CreateClient();
                httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
                var level = response2.SingleOrDefault(x=>x.LevelCode == selectedLevel.ToString());

                var apiGetDataShopProductByParentIdUrl = "https://localhost:7071/api/Sale/GetDataForShopProductByParentId/";
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = JsonConvert.DeserializeObject<ShopProductVMS>(httpClient.GetStringAsync(apiGetDataShopProductByParentIdUrl + selectedParent).Result);

                if (selectedLevel != null && level != null)
                {
                    selectedParent = int.Parse(level.ParentID);
                    var apiGetDataShopProductByLevelCode = "https://localhost:7071/api/Sale/GetDataForShopProductByLevelCode/";
                    var httpClient3 = _httpClientFactory.CreateClient();
                    httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    response = JsonConvert.DeserializeObject<ShopProductVMS>(httpClient3.GetStringAsync(apiGetDataShopProductByLevelCode + LevelCode).Result);
                }

                int priceFrom = 0;
                int priceTo = 0;

                var filter = response.ShopProducts.AsQueryable();

                if (priceF != null)
                {
                    priceFrom = int.Parse(priceF);
                    priceTo = int.Parse(priceT);
                    
                    filter = response.ShopProducts.AsQueryable().Where(x=>x.Price >= priceFrom && x.Price <= priceTo);

                    if (searchProduct != null)
                    {
                        filter = filter.Where(x=>x.ProductName.ToLower().Contains(searchProduct.ToLower()));
                    }

                    if (orderby != null)
                    {
                        if (orderby == "2")
                        {
                            filter = filter.OrderByDescending(x => x.Price);
                        }
                        else if(orderby == "3")
                        {
                            filter = filter.OrderBy(x => x.Price);
                        }
                        else if (orderby == "1")
                        {

                        }
                        else
                        {

                        }
                    }
                }

                PagedList.Core.PagedList<ViewModels.SaleClient.ShopProduct> models = new PagedList.Core.PagedList<ViewModels.SaleClient.ShopProduct>(priceF == null ? response.ShopProducts.AsQueryable() : filter, pageNumber, pageSize);
                ViewModels.SaleClient.ShopProductVM shopProductVM = new ViewModels.SaleClient.ShopProductVM();
                shopProductVM.ShopProducts = models;
                shopProductVM.RelatedShopProducts = response.RelatedShopProducts;
                shopProductVM.OrderBy = response.OrderBy;

                List<SelectListItem> parents = response.Parents.Select(n => new SelectListItem
                {
                    Value = n.ParentID,
                    Text = n.ParentName
                }).ToList();

                List<SelectListItem> levels = response.Levels.Select(n => new SelectListItem
                {
                    Value = n.LevelCode,
                    Text = n.LevelName
                }).ToList();

                shopProductVM.Parents = parents;

                shopProductVM.Levels = new List<SelectListItem>();

                if (selectedParent != 0 || level != null)
                {
                    shopProductVM.Levels = levels;
                }

                shopProductVM.Categories = new List<SelectListItem>();
                var totalSum = priceF != null ? filter.Count() : response.ShopProducts.Count();

                ViewBag.CurrentPage = pageNumber;
                ViewBag.totalBestSeller = totalSum;
                ViewBag.TotalP = totalSum;
                ViewBag.selectedParent = selectedParent;
                ViewBag.selectedLevel = selectedLevel;
                ViewBag.priceF = priceF;
                ViewBag.searchProduct = searchProduct;
                ViewBag.priceT = priceT;
                ViewData["top"] = selectedParent;
                return View(shopProductVM);
                //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_pvCateList", ProCateVM) });
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
