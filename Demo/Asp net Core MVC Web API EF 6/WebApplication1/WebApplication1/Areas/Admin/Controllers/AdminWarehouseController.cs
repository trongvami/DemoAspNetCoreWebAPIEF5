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
                    //return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 1, page = 1 });
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
                    //return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 1, page = 1 });
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
                //return RedirectToAction("ParentsList", "AdminWarehouse", new { area = "Admin" });
                return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 1, page = 1 });
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

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resParent = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient.GetStringAsync(apiGetAllParentUrl).Result);
            var queryableResParent = resParent.AsQueryable();
            ViewData["Parent"] = new SelectList(resParent, "ParentID", "ParentName");
            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Level")]
        public async Task<IActionResult> CreateLevel([Bind("LevelCode,ParentID,LevelName,LevelActive,LevelDelete")] LevelsListViewModel level)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                level.LevelDelete = 0;

                var apiAddNewLevelUrl = "https://localhost:7071/api/Warehouse/AddNewLevel";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewLevelUrl, level, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Level Successfully !", 4);
                    //return RedirectToAction("LevelsList", "AdminWarehouse", new { area = "Admin" });
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 2, page = 1 });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(level);
        }

        [Route("Admin/AdminWarehouse/Update-Level/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateLevel(string? id)
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

            var data = response.SingleOrDefault(x => x.LevelCode == id);

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resParent = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient2.GetStringAsync(apiGetAllParentUrl).Result);
            var queryableResParent = resParent.AsQueryable();
            ViewData["Parent"] = new SelectList(resParent, "ParentID", "ParentName");

            return View(data);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Update-Level/{id}")]
        public async Task<IActionResult> UpdateLevel(string? id, [Bind("LevelCode,ParentID,LevelName,LevelActive,LevelDelete")] LevelsListViewModel level)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                level.LevelCode = id;

                var apiUpdateLevelUrl = "https://localhost:7071/api/Warehouse/UpdateLevel";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateLevelUrl, level, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Level Successfully !", 4);
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 2, page = 1 });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View(level);
            }
            return View(level);
        }

        [Route("Admin/AdminWarehouse/Delete-Level/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteLevel(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiDeleteLevelUrl = "https://localhost:7071/api/Warehouse/DeleteLevel/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteLevelUrl, token, id);

            if (rs.IsSuccessStatusCode)
            {
                _notyfService.Success("Delete Level Successfully !", 4);
                return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 2, page = 1 });
            }
            else
            {
                _notyfService.Warning("Try again !", 4);
                return View();
            }
        }

        #endregion

        #region Unit Payment

        [Route("Admin/AdminWarehouse/Unitpays-List")]
        [HttpGet]
        public async Task<IActionResult> UnitpaysList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllUnitpayUrl = "https://localhost:7071/api/Warehouse/UnitPaymentsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<UnitPaymentsListViewModel>>(httpClient.GetStringAsync(apiGetAllUnitpayUrl).Result);
            var queryableResponse = response.AsQueryable();
            PagedList.Core.PagedList<UnitPaymentsListViewModel> models = new PagedList.Core.PagedList<UnitPaymentsListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("Admin/AdminWarehouse/Add-New-Unitpay")]
        [HttpGet]
        public async Task<IActionResult> CreateUnitpay()
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Unitpay")]
        public async Task<IActionResult> CreateUnitpay([Bind("UpayId,UpayName,IsActive,IsDelete")] UnitPaymentsListViewModel unit)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                unit.IsDelete = 0;

                var apiAddNewUnitpayUrl = "https://localhost:7071/api/Warehouse/AddNewUnitpay";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewUnitpayUrl, unit, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Unitpay Successfully !", 4);
                    return RedirectToAction("UnitpaysList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(unit);
        }

        [Route("Admin/AdminWarehouse/Update-Unitpay/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateUnitpay(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllUnitpayUrl = "https://localhost:7071/api/Warehouse/UnitPaymentsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<UnitPaymentsListViewModel>>(httpClient.GetStringAsync(apiGetAllUnitpayUrl).Result);
            var queryableResponse = response.AsQueryable();

            var data = response.SingleOrDefault(x => x.UpayId == id);

            return View(data);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Update-Unitpay/{id}")]
        public async Task<IActionResult> UpdateUnitpay(string? id, [Bind("UpayId,UpayName,IsActive,IsDelete")] UnitPaymentsListViewModel unit)
        {
            if (ModelState.IsValid)
            {

                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                unit.UpayId = id;
                unit.IsDelete = 0;

                var apiUpdateUnitpayUrl = "https://localhost:7071/api/Warehouse/UpdateUnitpay";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateUnitpayUrl, unit, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Unitpay Successfully !", 4);
                    return RedirectToAction("UnitpaysList", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            return View(unit);
        }

        [Route("Admin/AdminWarehouse/Delete-Unitpay/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteUnitpay(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiDeleteUnitpayUrl = "https://localhost:7071/api/Warehouse/DeleteUnitpay/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteUnitpayUrl, token, id);

            if (rs.IsSuccessStatusCode)
            {
                _notyfService.Success("Delete Unitpay Successfully !", 4);
                return RedirectToAction("UnitpaysList", "AdminWarehouse", new { area = "Admin" });
            }
            else
            {
                _notyfService.Warning("Try again !", 4);
                return View();
            }
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
                unit.IsDelete = 0;

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

        [Route("Admin/AdminWarehouse/Categories-List-2")]
        [HttpGet]
        public async Task<IActionResult> CategoriesList2(int? tab,int? page)
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
            PagedList.Core.PagedList<CategoriesListViewModel> modelsCate = new PagedList.Core.PagedList<CategoriesListViewModel>(queryableResponse, pageNumber, pageSize);

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
            var queryableResponse2 = response2.AsQueryable();
            PagedList.Core.PagedList<LevelsListViewModel> modelsLevel = new PagedList.Core.PagedList<LevelsListViewModel>(queryableResponse2, pageNumber, pageSize);

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient3 = _httpClientFactory.CreateClient();
            httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient3.GetStringAsync(apiGetAllParentUrl).Result);
            var queryableResponse3 = response3.AsQueryable();
            PagedList.Core.PagedList<ParentsListViewModel> modelsParent = new PagedList.Core.PagedList<ParentsListViewModel>(queryableResponse3, pageNumber, pageSize);

            CategoryViewModel models = new CategoryViewModel();
            models.pagedCategories = modelsCate;
            models.pagedLevels = modelsLevel;
            models.pagedParents = modelsParent;

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Tab = tab == null ? 1 : tab;
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

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
            var queryableResponse2 = response2.AsQueryable();
            ViewData["Level"] = new SelectList(queryableResponse2, "LevelCode", "LevelName");

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

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
            var queryableResponse2 = response2.AsQueryable();
            ViewData["Level"] = new SelectList(queryableResponse2, "LevelCode", "LevelName");

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
                category.ParentID = 0;

                var apiUpdateCategoryUrl = "https://localhost:7071/api/Warehouse/UpdateCategory";
                var response = await HttpClientHelper.PutWithTokenAsync(apiUpdateCategoryUrl, category, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update Category Successfully !", 4);
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 3, page = 1 });
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

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient.GetStringAsync(apiGetAllLevelUrl).Result);
            var queryableResponse = response.AsQueryable();
            ViewData["Level"] = new SelectList(response, "LevelCode", "LevelName");
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
                category.ParentID = 0;

                var apiAddNewCategoryUrl = "https://localhost:7071/api/Warehouse/AddNewCategory";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewCategoryUrl, category, token);

                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New Category Successfully !", 4);
                    return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 3, page = 1 });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View(category);
            }
            return View(category);
        }

        [Route("Admin/AdminWarehouse/Delete-Category/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiDeleteCategoryUrl = "https://localhost:7071/api/Warehouse/DeleteCategory/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteCategoryUrl, token, id);

            if (rs.IsSuccessStatusCode)
            {
                _notyfService.Success("Delete Category Successfully !", 4);
                return RedirectToAction("CategoriesList2", "AdminWarehouse", new { area = "Admin", tab = 3, page = 1 });
            }
            else
            {
                _notyfService.Warning("Try again !", 4);
                return View();
            }
        }

        #endregion

        #region Product

        [Route("Admin/AdminWarehouse/GetCategories")]
        [HttpGet]
        public async Task<IActionResult> GetCategories(string? selectedLevel)
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
            if (!string.IsNullOrWhiteSpace(selectedLevel)) {
                if (selectedLevel == "0")
                {
                    List<SelectListItem> categories = response.Select(
                    n => new SelectListItem
                    {
                        Value = n.CatId,
                        Text = n.CatName
                    }).ToList();
                    return Json(categories);
                }
                else
                {
                    List<SelectListItem> categories = response.Where(c => c.Levels.ToString() == selectedLevel).Select(
                    n => new SelectListItem
                    {
                        Value = n.CatId,
                        Text = n.CatName
                    }).ToList();
                    return Json(categories);
                }
            }

            return null;
        }

        [Route("Admin/AdminWarehouse/GetLevels")]
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

        [Route("Admin/AdminWarehouse/Products-List")]
        [HttpGet]
        public async Task<IActionResult> ProductsList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            //var pageSize = 20;
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

        [Route("Admin/AdminWarehouse/Products-List-2")]
        [HttpGet]
        public async Task<IActionResult> ProductsList2(int? page, string? searchTop, string? searchToc, string? searchCat, string? searchProd)
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Utilities.PAGE_SIZE;
            var pageSize = 20;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
            var httpClient1 = _httpClientFactory.CreateClient();
            httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resParents = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient1.GetStringAsync(apiGetAllParentUrl).Result);
            List<SelectListItem> Parents = resParents.Select(n => new SelectListItem
            {
                Value = n.ParentID,
                Text = n.ParentName
            }).ToList();

            var apiGetAllProductUrl = "https://localhost:7071/api/Warehouse/ProductsList2";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<ProductsListViewModel2>>(httpClient.GetStringAsync(apiGetAllProductUrl).Result);
            var queryableResponse = response.AsQueryable();

            if (searchTop != null)
            {
                if (searchTop != "0")
                {
                    queryableResponse = queryableResponse.Where(x => x.Level.ParentId.ToString() == searchTop);
                }
            }

            if (searchToc != null)
            {
                if (searchToc != "0")
                {
                    queryableResponse = queryableResponse.Where(x => x.LevelCode == searchToc);
                }
            }

            if (searchCat != null)
            {
                if (searchCat != "0")
                {
                    queryableResponse = queryableResponse.Where(x => x.CatID == searchCat);
                }
            }

            if (searchProd != null)
            {
                queryableResponse = queryableResponse.Where(x => x.ProductName.Contains(searchProd));
            }

            PagedList.Core.PagedList<ProductsListViewModel2> models = new PagedList.Core.PagedList<ProductsListViewModel2>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            productsViewModel.lsProduct = models;
            productsViewModel.Parents = Parents;

            if (searchToc != null)
            {
                var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
                var httpClient2 = _httpClientFactory.CreateClient();
                httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resParents2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
                List<SelectListItem> Levels = resParents2.Select(n => new SelectListItem
                {
                    Value = n.LevelCode,
                    Text = n.LevelName
                }).ToList();
                productsViewModel.Levels = Levels;
            }
            else
            {
                productsViewModel.Levels = new List<SelectListItem>();
            }

            if (searchCat != null)
            {
                var apiGetAllCateUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
                var httpClient3 = _httpClientFactory.CreateClient();
                httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resParents3 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient3.GetStringAsync(apiGetAllCateUrl).Result);
                List<SelectListItem> Categories = resParents3.Select(n => new SelectListItem
                {
                    Value = n.CatId,
                    Text = n.CatName
                }).ToList();
                productsViewModel.Categories = Categories;
            }
            else
            {
                if (searchToc != null)
                {
                    var apiGetAllCateUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
                    var httpClient3 = _httpClientFactory.CreateClient();
                    httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resParents3 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient3.GetStringAsync(apiGetAllCateUrl).Result);
                    var dataCheck = resParents3.Where(x => x.Levels.ToString() == searchToc);
                    var dataCheck2 = resParents3.Where(x => x.Levels.ToString() == searchToc).ToList();
                    
                    if (dataCheck2.Count > 0)
                    {
                        List<SelectListItem> Categories = dataCheck.Select(n => new SelectListItem
                        {
                            Value = n.CatId,
                            Text = n.CatName
                        }).ToList();
                        productsViewModel.Categories = Categories;
                    }
                    else { productsViewModel.Categories = new List<SelectListItem>(); }
                }
                else
                {
                    productsViewModel.Categories = new List<SelectListItem>();
                }
            }

            ViewBag.CurrentCatID = searchCat;
            ViewBag.CurrentLevelCode = searchToc;
            ViewBag.CurrentParentID = searchTop;
            ViewBag.CurrentSearchProd = searchProd;

            return View(productsViewModel);
        }

        //[Route("Admin/AdminWarehouse/Filter-Product")]
        //[HttpGet]
        //public async Task<IActionResult> FilterProduct(int? page, string? searchTop, string? searchToc, string? searchCat, string? searchProd)
        //{
        //    ProductsViewModel productsViewModel = new ProductsViewModel();
        //    var pageNumber = page == null || page <= 0 ? 1 : page.Value;
        //    //var pageSize = Utilities.PAGE_SIZE;
        //    var pageSize = 20;
        //    var token = Request.Cookies["IdentityToken"];

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
        //    }

        //    var apiGetAllParentUrl = "https://localhost:7071/api/Warehouse/ParentsList";
        //    var httpClient1 = _httpClientFactory.CreateClient();
        //    httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var resParents = JsonConvert.DeserializeObject<List<ParentsListViewModel>>(httpClient1.GetStringAsync(apiGetAllParentUrl).Result);
        //    List<SelectListItem> Parents = resParents.Select(n => new SelectListItem
        //    {
        //        Value = n.ParentID,
        //        Text = n.ParentName
        //    }).ToList();

        //    var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
        //    var httpClient2 = _httpClientFactory.CreateClient();
        //    httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var resParents2 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient2.GetStringAsync(apiGetAllLevelUrl).Result);
        //    List<SelectListItem> Levels = resParents2.Select(n => new SelectListItem
        //    {
        //        Value = n.LevelCode,
        //        Text = n.LevelName
        //    }).ToList();

        //    var apiGetAllCateUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
        //    var httpClient3 = _httpClientFactory.CreateClient();
        //    httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var resParents3 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient3.GetStringAsync(apiGetAllCateUrl).Result);
        //    List<SelectListItem> Categories = resParents3.Select(n => new SelectListItem
        //    {
        //        Value = n.CatId,
        //        Text = n.CatName
        //    }).ToList();

        //    var apiGetAllProductUrl = "https://localhost:7071/api/Warehouse/ProductsList2";
        //    var httpClient = _httpClientFactory.CreateClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = JsonConvert.DeserializeObject<List<ProductsListViewModel2>>(httpClient.GetStringAsync(apiGetAllProductUrl).Result);
        //    var queryableResponse = response.AsQueryable();

        //    if (searchTop != null)
        //    {
        //        queryableResponse = queryableResponse.Where(x => x.Level.ParentId.ToString() == searchTop);
        //    }

        //    if (searchToc != null)
        //    {
        //        queryableResponse = queryableResponse.Where(x => x.LevelCode == searchToc);
        //    }

        //    if (searchCat != null)
        //    {
        //        queryableResponse = queryableResponse.Where(x=>x.CatID == searchCat);
        //    }

        //    if (searchProd != null)
        //    {
        //        queryableResponse = queryableResponse.Where(x=>x.ProductName.Contains(searchProd));
        //    }

        //    PagedList.Core.PagedList<ProductsListViewModel2> models = new PagedList.Core.PagedList<ProductsListViewModel2>(queryableResponse, pageNumber, pageSize);
        //    ViewBag.CurrentPage = pageNumber;

        //    productsViewModel.lsProduct = models;
        //    productsViewModel.Parents = Parents;

        //    if (searchToc != null)
        //    {
        //        productsViewModel.Levels = Levels;
        //    }
        //    else
        //    {
        //        productsViewModel.Levels = new List<SelectListItem>();
        //    }

        //    if (searchCat != null)
        //    {
        //        productsViewModel.Categories = Categories;
        //    }
        //    else
        //    {
        //        productsViewModel.Categories = new List<SelectListItem>();
        //    }

        //    ViewBag.CurrentCatID = searchCat;
        //    ViewBag.CurrentLevelCode = searchToc;
        //    ViewBag.CurrentParentID = searchTop;
        //    ViewBag.CurrentSearchProd = searchProd;

        //    return View(productsViewModel);
        //}

        [Route("Admin/AdminWarehouse/Add-New-Product")]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
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

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient2.GetStringAsync(apiGetAllRoleUrl).Result);

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient3 = _httpClientFactory.CreateClient();
            httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient3.GetStringAsync(apiGetAllUnitUrl).Result);

            var apiGetAllUnitpayUrl = "https://localhost:7071/api/Warehouse/UnitPaymentsList";
            var httpClient4 = _httpClientFactory.CreateClient();
            httpClient4.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response4 = JsonConvert.DeserializeObject<List<UnitPaymentsListViewModel>>(httpClient4.GetStringAsync(apiGetAllUnitpayUrl).Result);

            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel();
            productCreateViewModel.Product = new ProductsListViewModel();
            List<SelectListItem> levels = response.Select(n=> new SelectListItem { 
                Value = n.LevelCode,
                Text = n.LevelName
            }).ToList();
            List<SelectListItem> units = response3.Select(n => new SelectListItem
            {
                Value = n.UnitID,
                Text = n.UnitName
            }).ToList();
            List<SelectListItem> unitpays = response4.Select(n => new SelectListItem
            {
                Value = n.UpayId,
                Text = n.UpayName
            }).ToList();

            productCreateViewModel.Levels = levels;
            productCreateViewModel.Categories = new List<SelectListItem>();
            productCreateViewModel.Units = units;
            productCreateViewModel.Unitpays = unitpays;

            return View(productCreateViewModel);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Add-New-Product")]
        public async Task<IActionResult> CreateProduct([Bind("ProductId,ProductName,ShortDesc,Description,CatID,LevelCode,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,SoLuongBanDau,SoLuongDaBan,UnitsInStock,UnitID,UpayId,Height,Image1,Image2,Image3,Image4,Image5,Image6")] ProductsListViewModel product, Microsoft.AspNetCore.Http.IFormFile? fThumb, List<IFormFile> fThumb2, string? levelCode)
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
                if (product.UnitID == 22 || product.Height == null)
                {
                    product.Height = 0;
                }
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

        [Route("Admin/AdminWarehouse/Edit-Product")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetProductByIdUrl = "https://localhost:7071/api/Warehouse/ProductById/";
            var httpClient5 = _httpClientFactory.CreateClient();
            httpClient5.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response5 = JsonConvert.DeserializeObject<ProductsListViewModel>(httpClient5.GetStringAsync(apiGetProductByIdUrl + id).Result);

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient.GetStringAsync(apiGetAllLevelUrl).Result);

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient2.GetStringAsync(apiGetAllRoleUrl).Result);

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient3 = _httpClientFactory.CreateClient();
            httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient3.GetStringAsync(apiGetAllUnitUrl).Result);

            var apiGetAllUnitpayUrl = "https://localhost:7071/api/Warehouse/UnitPaymentsList";
            var httpClient4 = _httpClientFactory.CreateClient();
            httpClient4.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response4 = JsonConvert.DeserializeObject<List<UnitPaymentsListViewModel>>(httpClient4.GetStringAsync(apiGetAllUnitpayUrl).Result);

            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel();
            productCreateViewModel.Product = response5;
            List<SelectListItem> levels = response.Select(n => new SelectListItem
            {
                Value = n.LevelCode,
                Text = n.LevelName
            }).ToList();
            List<SelectListItem> units = response3.Select(n => new SelectListItem
            {
                Value = n.UnitID,
                Text = n.UnitName
            }).ToList();
            List<SelectListItem> cates = response2.Select(n => new SelectListItem
            {
                Value = n.CatId,
                Text = n.CatName
            }).ToList();
            List<SelectListItem> unitpays = response4.Select(n => new SelectListItem
            {
                Value = n.UpayId,
                Text = n.UpayName
            }).ToList();

            productCreateViewModel.Levels = levels;
            productCreateViewModel.Categories = cates;
            productCreateViewModel.Units = units;
            productCreateViewModel.Unitpays = unitpays;

            return View(productCreateViewModel);
        }

        [HttpPost]
        [Route("Admin/AdminWarehouse/Edit-Product")]
        public async Task<IActionResult> EditProduct(string? id, [Bind("ProductId,ProductName,ShortDesc,Description,CatID,LevelCode,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,SoLuongBanDau,SoLuongDaBan,UnitsInStock,UnitID,UpayId,Height,Image1,Image2,Image3,Image4,Image5,Image6")] ProductsListViewModel product, Microsoft.AspNetCore.Http.IFormFile? fThumb, List<IFormFile> fThumb2, string? levelCode)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            //var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            //var httpClient = _httpClientFactory.CreateClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var response = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            //var queryableResponse = response.AsQueryable();

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
                if (fThumb2 != null)
                {
                    if (fThumb2.Count > 0)
                    {
                        check = check + 1;
                        foreach (var item in fThumb2)
                        {
                            string extension = Path.GetExtension(item.FileName);
                            string image = Utilities.SEOUrl(product.ProductName) + extension;
                            if (check == 1)
                            {
                                product.Image1 = await Utilities.UploadFile(item, @"imgsProduct", image.ToLower());
                            }
                            else if (check == 2)
                            {
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

                product.ProductId = id;
                if (product.MetaDesc == null || product.MetaDesc == "")
                {
                    product.MetaDesc = product.ProductName;
                }

                if (product.MetaKey == null || product.MetaKey == "")
                {
                    product.MetaKey = product.ProductName;
                }

                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.UtcNow.Date;
                if (product.UnitID == 22 || product.Height == null)
                {
                    product.Height = 0;
                }

                var apiEditProductUrl = "https://localhost:7071/api/Warehouse/EditProduct";
                var res = await HttpClientHelper.PostWithTokenAsync(apiEditProductUrl, product, token);

                if (res.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add Edit Product Successfully !", 4);
                    return RedirectToAction("ProductsList2", "AdminWarehouse", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
            }

            var apiGetAllLevelUrl = "https://localhost:7071/api/Warehouse/LevelsList";
            var httpClient5 = _httpClientFactory.CreateClient();
            httpClient5.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response5 = JsonConvert.DeserializeObject<List<LevelsListViewModel>>(httpClient5.GetStringAsync(apiGetAllLevelUrl).Result);

            var apiGetAllRoleUrl = "https://localhost:7071/api/Warehouse/CategoriesList";
            var httpClient2 = _httpClientFactory.CreateClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = JsonConvert.DeserializeObject<List<CategoriesListViewModel>>(httpClient2.GetStringAsync(apiGetAllRoleUrl).Result);

            var apiGetAllUnitUrl = "https://localhost:7071/api/Warehouse/UnitsList";
            var httpClient3 = _httpClientFactory.CreateClient();
            httpClient3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = JsonConvert.DeserializeObject<List<UnitsListViewModel>>(httpClient3.GetStringAsync(apiGetAllUnitUrl).Result);

            var apiGetAllUnitpayUrl = "https://localhost:7071/api/Warehouse/UnitPaymentsList";
            var httpClient4 = _httpClientFactory.CreateClient();
            httpClient4.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response4 = JsonConvert.DeserializeObject<List<UnitPaymentsListViewModel>>(httpClient4.GetStringAsync(apiGetAllUnitpayUrl).Result);

            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel();
            productCreateViewModel.Product = product;
            List<SelectListItem> levels = response5.Select(n => new SelectListItem
            {
                Value = n.LevelCode,
                Text = n.LevelName
            }).ToList();
            List<SelectListItem> units = response3.Select(n => new SelectListItem
            {
                Value = n.UnitID,
                Text = n.UnitName
            }).ToList();
            List<SelectListItem> unitpays = response4.Select(n => new SelectListItem
            {
                Value = n.UpayId,
                Text = n.UpayName
            }).ToList();

            productCreateViewModel.Levels = levels;
            productCreateViewModel.Categories = new List<SelectListItem>();
            productCreateViewModel.Units = units;
            productCreateViewModel.Unitpays = unitpays;

            return View(productCreateViewModel);
        }

        #endregion

    }
}
