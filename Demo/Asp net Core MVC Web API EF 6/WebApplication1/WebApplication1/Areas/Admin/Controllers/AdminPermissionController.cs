using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using NuGet.Common;
using System.Drawing;
using System.Drawing.Printing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebApplication1.Common;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminPermissionController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpClientFactory _httpClientFactory;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;
        public AdminPermissionController(IHttpClientFactory httpClientFactory, IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _httpClientFactory = httpClientFactory;
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }

        #region Role
        [Route("Admin/AdminPermission/Role-List")]
        [HttpGet]
        public async Task<IActionResult> RoleList(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/RoleList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            //response.OrderByDescending(x=>x.ConcurrencyStamp);
            //var queryableResponse = response.AsQueryable().OrderByDescending(x => x.ConcurrencyStamp);
            var queryableResponse = response.AsQueryable().OrderBy(x => x.Name);
            PagedList.Core.PagedList<RoleListViewModel> models = new PagedList.Core.PagedList<RoleListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            //_toastNotification.AddSuccessToastMessage("Test Notification !");
            //_toastNotification.AddInfoToastMessage("Here is some information.");
            //_toastNotification.AddErrorToastMessage("Woops an error occured.");
            //_toastNotification.AddWarningToastMessage("Here is a simple warning!");

            if (response != null)
            {
                return View(models);
            }
            else
            {
                return View(models);
            }
        }

        [Route("Admin/AdminPermission/Update-Role")]
        [HttpGet]
        public async Task<IActionResult> AddorEdit(string id)
        {
            if (id == "" || id == null)
            {
                return View(new RoleListViewModel());
            }
            else
            {
                var token = Request.Cookies["IdentityToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }

                var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/GetRoleById/";
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = JsonConvert.DeserializeObject<RoleListViewModel>(httpClient.GetStringAsync(apiGetAllRoleUrl + id).Result);
                //var model = JsonConvert.DeserializeObject<RoleListViewModel>(client.GetStringAsync(urlAPICategory + id).Result);
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
        }

        [Route("Admin/AdminPermission/Update-Role")]
        [HttpPost]
        public async Task<IActionResult> AddorEdit(string id, [Bind("Id, Name, ConcurrencyStamp")] RoleListViewModel roleListViewModel)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }
            if (id == null || id == "")
            {
                var apiAddNewRoleUrl = "https://localhost:7071/api/RolePermission/AddRole";
                var response = await HttpClientHelper.PostWithTokenAsync(apiAddNewRoleUrl, roleListViewModel, token);
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Add New This Role: " + roleListViewModel.Name + " Successfully !", 4);
                    return RedirectToAction("RoleList", "AdminPermission", new { area = "Admin" });

                    //var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/RoleList";
                    //var httpClient = _httpClientFactory.CreateClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //var rs = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
                    //rs.OrderByDescending(x => x.ConcurrencyStamp);
                    //var queryableResponse = rs.AsQueryable().OrderByDescending(x => x.ConcurrencyStamp);
                    //PagedList.Core.PagedList<RoleListViewModel> models = new PagedList.Core.PagedList<RoleListViewModel>(queryableResponse, 1, 4);
                    //ViewBag.CurrentPage = 1;
                    //return Json(new { isValid = true, html = RenderHelper.RenderRazorViewToString(this, "_pvRoleList", models) });

                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
            else
            {
                var apiAddNewRoleUrl = "https://localhost:7071/api/RolePermission/UpdateRole";
                var response = await HttpClientHelper.PutWithTokenAsync(apiAddNewRoleUrl, roleListViewModel, token);
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Update This Role: " + roleListViewModel.Name + " Successfully !", 4);
                    return RedirectToAction("RoleList", "AdminPermission", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Something is wrong", 4);
                }
                return View();
            }
        }

        [Route("Admin/AdminPermission/DeleteRole/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string? id)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/GetRoleById/";
            var apiDeleteRoleUrl = "https://localhost:7071/api/RolePermission/DeleteRole/";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<RoleListViewModel>(httpClient.GetStringAsync(apiGetAllRoleUrl + id).Result);

            if (response == null)
            {
                _notyfService.Warning($"The Role {response.Name} doesn't exist", 4);
                return View();
            }
            else
            {
                var rs = await HttpClientHelper.DeleteWithTokenAndIdAsync(apiDeleteRoleUrl, token, id);
                if (rs.IsSuccessStatusCode)
                {
                    _notyfService.Success("Delete Role: " + response.Name + " Successfully !", 4);
                    return RedirectToAction("RoleList", "AdminPermission", new { area = "Admin" });
                }
                else
                {
                    _notyfService.Warning("Try again !", 4);
                    return View();
                }
            }
        }
        #endregion

        #region Employee
        [Route("Admin/AdminPermission/Employee-List")]
        [HttpGet]
        public async Task<IActionResult> EmployeeList(int? page)
        {
            //var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Utilities.PAGE_SIZE;
            //var token = Request.Cookies["IdentityToken"];

            //if (string.IsNullOrEmpty(token))
            //{
            //    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            //}

            //var apiGetAllEmployeeUrl = "https://localhost:7071/api/RolePermission/EmployeeList";
            //var httpClient = _httpClientFactory.CreateClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var response = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllEmployeeUrl).Result);

            //var queryableResponse = response.AsQueryable().OrderBy(x => x.Name);
            //PagedList.Core.PagedList<RoleListViewModel> models = new PagedList.Core.PagedList<RoleListViewModel>(queryableResponse, pageNumber, pageSize);
            //ViewBag.CurrentPage = pageNumber;

            //if (response != null)
            //{
            //    return View(models);
            //}
            //else
            //{
            //    return View(models);
            //}
            return View();
        }
        #endregion

        #region Claim
        [Route("Admin/AdminPermission/Claim-List")]
        [HttpGet]
        public async Task<IActionResult> ClaimList(string? idRole)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/RoleList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var firstData = response.FirstOrDefault();

            if (idRole == null)
            {
                idRole = firstData.Id;
            }
            ViewBag.Id = idRole;

            var apiGetAllRoleClaimsUrl = "https://localhost:7071/api/RolePermission/GetClaimsByRoleId/";
            var claimsRoleData = JsonConvert.DeserializeObject<RoleClaimsViewModel>(httpClient.GetStringAsync(apiGetAllRoleClaimsUrl + idRole).Result);

            return View(claimsRoleData);
        }

        [Route("Admin/AdminPermission/ClaimsListByRoleId/{id}")]
        [HttpGet]
        public async Task<IActionResult> ClaimsListByRoleId(string? idRole)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            var apiGetAllRoleUrl = "https://localhost:7071/api/RolePermission/RoleList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            var firstData = response.FirstOrDefault();

            if (idRole == null)
            {
                idRole = firstData.Id;
            }
            ViewBag.Id = idRole;

            var apiGetAllRoleClaimsUrl = "https://localhost:7071/api/RolePermission/GetClaimsByRoleId/";
            var claimsRoleData = JsonConvert.DeserializeObject<RoleClaimsViewModel>(httpClient.GetStringAsync(apiGetAllRoleClaimsUrl + idRole).Result);
            //_notyfService.Success("Load Successfully !", 4);
            return Json(new { isValid = true, html = RenderHelper.RenderRazorViewToString(this, "_pvClaimsList", claimsRoleData) });
        }

        [Route("Admin/AdminPermission/UpdateClaimByRoleId")]
        [HttpPost]
        public async Task<IActionResult> UpdateClaimByRoleId(string roleId, string[] IsSelected, List<ClaimsViewModel> listClaims)
        {
            var token = Request.Cookies["IdentityToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            ClaimsModelUpdate cmUpdate = new ClaimsModelUpdate();
            cmUpdate.roleId = roleId;
            cmUpdate.claimsList = IsSelected.ToList();

            var apiUpdateClaimsUrl = "https://localhost:7071/api/RolePermission/UpdateClaims";
            var response = await HttpClientHelper.PostWithTokenAsync(apiUpdateClaimsUrl, cmUpdate, token);
            if (response.IsSuccessStatusCode)
            {
                _notyfService.Success("Add Claims Successfully !", 4);
                return RedirectToAction("ClaimList", "AdminPermission", new { area = "Admin" });
            }else
            {
                _notyfService.Warning("Something is wrong", 4);
            }
            return View();
        }
        #endregion

        #region UsersInRole
        [Route("Admin/AdminPermission/Users-In-Role")]
        [HttpGet]
        public async Task<IActionResult> UsersInRole()
        {
            var claimList = ClaimsStore.AllClaims.ToList();
            return View(claimList);
        }
        #endregion
    }
}
