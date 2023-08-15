using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using NuGet.Common;
using System.Net.Http.Headers;
using WebApplication1.Common;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminWarehouseController : Controller
    {
        //private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpClientFactory _httpClientFactory;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;

        public AdminWarehouseController(IHttpClientFactory httpClientFactory, IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _httpClientFactory = httpClientFactory;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }

        #region Parent

        [Route("Admin/AdminWarehouse/Parents-List")]
        [HttpGet]
        public async Task<IActionResult> ParentsList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient.GetStringAsync(apiGetAllParentUrl).Result);
            var queryableResponse = response.AsQueryable();
            PagedList.Core.PagedList<ParentsListViewModel> models = new PagedList.Core.PagedList<ParentsListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Add-New-Parent")]
        [HttpGet]
        public async Task<IActionResult> CreateParent()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Parent")]
        public async Task<IActionResult> CreateParent([Bind("ParentID,ParentName,ParentActive,ParentDelete")] ParentsListViewModel parent)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                parent.ParentDelete = 0;

                var apiAddNewParentUrl = "https://localhost:7071/api/Warehouse/AddNewParent";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewParentUrl, parent, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Parent Successfully !", 4);
                    return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(parent);
        }

        [Route("Admin/AdminWarehouse/Update-Parent/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateParent(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient.GetStringAsync(apiGetAllParentUrl).Result);
            var queryableResponse = response.AsQueryable();

            var data = response.SingleOrDefault(x => x.ParentID == id);

            return View(data);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Update-Parent/{id}")]
        public async Task<IActionResult> UpdateParent(string? id, [Bind("ParentID,ParentName,ParentActive,ParentDelete")] ParentsListViewModel parent)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                parent.ParentID = id;

                var apiUpdateParentUrl = "https://localhost:7071/api/Warehouse/UpdateParent";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateParentUrl, parent, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Parent Successfully !", 4);
                    return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(parent);
        }

        [Route("Admin/AdminWarehouse/Delete-Parent/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteParent(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiDeleteParentUrl = "https://localhost:7071/api/Warehouse/DeleteParent/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteParentUrl, token, id);

            if (rs.IsSuccessStatusCode)
            {
                _notyfService.Success("Delete Parent Successfully !", 4);
                return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
            }
            else
            {
                _notyfService.Warning("Try again !", 4);
                return View();
            }
        }

        #endregion

        #region Level

        [Route("Admin/AdminWarehouse/Levels-List")]
        [HttpGet]
        public async Task<IActionResult> LevelsList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
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
            PagedList.Core.PagedList<LevelsListViewModel> models = new PagedList.Core.PagedList<LevelsListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Add-New-Level")]
        [HttpGet]
        public async Task<IActionResult> CreateLevel()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Level")]
        public async Task<IActionResult> CreateLevel([Bind("ParentID,ParentName,ParentActive,ParentDelete")] LevelsListViewModel level)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                level.LevelDelete = 0;

                var apiAddNewParentUrl = "https://localhost:7071/api/Warehouse/AddNewParent";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewParentUrl, level, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Level Successfully !", 4);
                    return RedirectToAction("LevelsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(level);
        }

        #endregion

        #region Unit

        [Route("Admin/AdminWarehouse/Units-List")]
        [HttpGet]
        public async Task<IActionResult> UnitsList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient.GetStringAsync(apiGetAllUnitUrl).Result);
            var queryableResponse = response.AsQueryable();
            PagedList.Core.PagedList<UnitsListViewModel> models = new PagedList.Core.PagedList<UnitsListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Add-New-Unit")]
        [HttpGet]
        public async Task<IActionResult> CreateUnit()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Unit")]
        public async Task<IActionResult> CreateUnit([Bind("UnitID,UnitName,IsActive,IsDelete")] UnitsListViewModel unit)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }
              
                unit.IsDelete = 0;

                var apiAddNewUnitUrl = "https://localhost:7071/api/Warehouse/AddNewUnit";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewUnitUrl, unit, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Unit Successfully !", 4);
                    return RedirectToAction("UnitsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(unit);
        }

        [Route("Admin/AdminWarehouse/Update-Unit/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateUnit(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient.GetStringAsync(apiGetAllUnitUrl).Result);
            var queryableResponse = response.AsQueryable();

            var data = response.SingleOrDefault(x=>x.UnitID == id);

            return View(data);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Update-Unit/{id}")]
        public async Task<IActionResult> UpdateUnit(string? id, [Bind("UnitID,UnitName,IsActive,IsDelete")] UnitsListViewModel unit)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                unit.UnitID = id;

                var apiUpdateUnitUrl = "https://localhost:7071/api/Warehouse/UpdateUnit";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateUnitUrl, unit, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Unit Successfully !", 4);
                    return RedirectToAction("UnitsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(unit);
        }

        [Route("Admin/AdminWarehouse/Delete-Unit/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteUnit(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiDeleteUnitUrl = "https://localhost:7071/api/Warehouse/DeleteUnit/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteUnitUrl, token, id);

            if (rs.IsSuccessStatusCode)
            {
                _notyfService.Success("Delete Unit Successfully !", 4);
                return RedirectToAction("UnitsList", "AdminWarehouse", new { area = "Admin" });
            }
            else
            {
                _notyfService.Warning("Try again !", 4);
                return View();
            }
        }

        #endregion

        #region Category

        [Route("Admin/AdminWarehouse/Categories-List")]
        [HttpGet]
        public async Task<IActionResult> CategoriesList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            
            var queryableResponse = response.AsQueryable();
            PagedList.Core.PagedList<CategoriesListViewModel> models = new PagedList.Core.PagedList<CategoriesListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Details-Category/{id}")]

        [HttpGet]
        public async Task<IActionResult> DetailsCategory(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var data = response.SingleOrDefault(x => x.CatId == id);
            return View(data);
        }

        [Route("Admin/AdminWarehouse/Edit-Category/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditCategory(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var data = response.SingleOrDefault(x => x.CatId == id);
            return View(data);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Edit-Category/{id}")]
        public async Task<IActionResult> EditCategory(string? id, [Bind("CatId,CatName,ShortContent,Description,ParentId,Levels,Ordering,Thumb,Title,Alias,Published,MetaDesc,MetaKey,SchemaMarkup")] CategoriesListViewModel category, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                //category.CatId = id;

                if (fThumb != null)
                {
                    category.CatName = Utilities.ToTitleCase(category.CatName);
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(category.CatName) + extension;
                    category.Thumb = await Utilities.UploadFile(fThumb, @"imgsCategory", image.ToLower());
                }
                if (string.IsNullOrEmpty(category.Thumb))
                {
                    category.Thumb = "default.jpg";
                }
                category.MetaDesc = category.CatName;
                category.MetaKey = category.CatName;
                category.SchemaMarkup = category.CatName;
                category.Alias = Utilities.SEOUrl(category.CatName);
                category.Levels = 0;
                category.ParentID = 0;

                var apiUpdateCategoryUrl = "https://localhost:7071/api/Warehouse/UpdateCategory";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateCategoryUrl, category, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Category Successfully !", 4);
                    return RedirectToAction("CategoriesList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(category);
        }

        [Route("Admin/AdminWarehouse/Add-New-Category")]
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Category")]
        public async Task<IActionResult> CreateCategory([Bind("CatId,CatName,ShortContent,Description,ParentId,Levels,Ordering,Thumb,Title,Alias,Published,MetaDesc,MetaKey,SchemaMarkup")] CategoriesListViewModel category, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                if (fThumb != null)
                {
                    category.CatName = Utilities.ToTitleCase(category.CatName);
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(category.CatName) + extension;
                    category.Thumb = await Utilities.UploadFile(fThumb, @"imgsCategory", image.ToLower());
                }
                if (string.IsNullOrEmpty(category.Thumb))
                {
                    category.Thumb = "default.jpg";
                }
                category.MetaDesc = category.CatName;
                category.MetaKey = category.CatName;
                category.SchemaMarkup = category.CatName;
                category.Alias = Utilities.SEOUrl(category.CatName);
                category.CatId = null;
                category.Levels = 0;
                category.ParentID = 0;

                var apiAddNewCategoryUrl = "https://localhost:7071/api/Warehouse/AddNewCategory";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewCategoryUrl, category, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Category Successfully !", 4);
                    return RedirectToAction("CategoriesList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(category);
        }
        #endregion

        #region Product

        [Route("Admin/AdminWarehouse/Products-List")]
        [HttpGet]
        public async Task<IActionResult> ProductsList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllProductUrl = "https://localhost:7071/api/Warehouse/ProductsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<ProductsListViewModel>>(httpClient.GetStringAsync(apiGetAllProductUrl).Result);
            var queryableResponse = response.AsQueryable();
            PagedList.Core.PagedList<ProductsListViewModel> models = new PagedList.Core.PagedList<ProductsListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Add-New-Product")]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var queryableResponse = response.AsQueryable();

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient.GetStringAsync(apiGetAllUnitUrl).Result);

            ViewData["DanhMuc"] = new SelectList(response, "CatId", "CatName");
            ViewData["DonVi"] = new SelectList(response2, "UnitID", "UnitName");
            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Product")]
        public async Task<IActionResult> CreateProduct([Bind("ProductId,ProductName,ShortDesc,Description,CatID,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,SoLuongBanDau,SoLuongDaBan,UnitsInStock,UnitID,Image1,Image2,Image3,Image4,Image5,Image6")] ProductsListViewModel product, Microsoft.AspNetCore.Http.IFormFile? fThumb, List<IFormFile> fThumb2)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var queryableResponse = response.AsQueryable();

            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.Thumb = await Utilities.UploadFile(fThumb, @"imgsProduct", image.ToLower());
                }

                int check = 0;
                if (fThumb2 != null) {
                    if (fThumb2.Count > 0) {
                        check = check + 1;
                        foreach (var item in fThumb2)
                        {
                            string extension = Path.GetExtension(item.FileName);
                            string image = Utilities.SEOUrl(product.ProductName) + extension;
                            if (check == 1) {
                                product.Image1 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            } else if (check == 2) {
                                product.Image2 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            }
                            else if (check == 3)
                            {
                                product.Image3 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            }
                            else if (check == 4)
                            {
                                product.Image4 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            }
                            else if (check == 5)
                            {
                                product.Image5 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            }
                            check = check + 1;
                        }
                    }
                }

                if (check == 0) {
                    product.Image1 = "default.jpg";
                    product.Image2 = "default.jpg";
                    product.Image3 = "default.jpg";
                    product.Image4 = "default.jpg";
                    product.Image5 = "default.jpg";
                    product.Image6 = "default.jpg";
                } else if (check == 2) {
                    product.Image2 = "default.jpg";
                    product.Image3 = "default.jpg";
                    product.Image4 = "default.jpg";
                    product.Image5 = "default.jpg";
                    product.Image6 = "default.jpg";
                }
                else if (check == 3)
                {
                    product.Image3 = "default.jpg";
                    product.Image4 = "default.jpg";
                    product.Image5 = "default.jpg";
                    product.Image6 = "default.jpg";
                }
                else if (check == 4)
                {
                    product.Image4 = "default.jpg";
                    product.Image5 = "default.jpg";
                    product.Image6 = "default.jpg";
                }
                else if (check == 5)
                {
                    product.Image5 = "default.jpg";
                    product.Image6 = "default.jpg";
                }
                else if (check == 6)
                {
                    product.Image6 = "default.jpg";
                }

                if (string.IsNullOrEmpty(product.Thumb))
                {
                    product.Thumb = "default.jpg";
                }

                product.ProductId = null;
                if (product.MetaDesc == null || product.MetaDesc == "") {
                    product.MetaDesc = product.ProductName;
                }
                if (product.MetaKey == null || product.MetaKey == "")
                {
                    product.MetaKey = product.ProductName;
                }

                product.Alias = Utilities.SEOUrl(product.ProductName);  
                product.DateCreated = DateTime.UtcNow.Date;
                product.DateModified = DateTime.UtcNow.Date;
                product.UnitsInStock = product.SoLuongBanDau;
                product.SoLuongDaBan = 0;

                var apiAddNewProductUrl = "https://localhost:7071/api/Warehouse/AddNewProduct";
                var res = await HttpClientHelper.PostWithTokenAsync(apiAddNewProductUrl, product, token);

                if (res.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Product Successfully !", 4);
                    return RedirectToAction("ProductsList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                    ViewData["DanhMuc"] = new SelectList(response, "CatId", "CatName");
                    return View(product);
                }
            }

            ViewData["DanhMuc"] = new SelectList(response, "CatId", "CatName");
            ViewData["DonVi"] = new SelectList(response, "UnitID", "UnitName");

            return View(product);
        }

        #endregion

    }
}
