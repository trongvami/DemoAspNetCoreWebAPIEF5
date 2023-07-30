using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Net.Http.Headers;
using WebApplication1.Common;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminPermissionController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpClientFactory _httpClientFactory;
        //public INotyfService _notyfService { get; set; }
        private readonly IToastNotification _toastNotification;
        public AdminPermissionController(IHttpClientFactory httpClientFactory, IToastNotification toastNotification)
        {
            _httpClientFactory = httpClientFactory;
            //_notyfService = notyfService;
            _toastNotification = toastNotification;
        }

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
            response.OrderByDescending(x=>x.ConcurrencyStamp);
            var queryableResponse = response.AsQueryable().OrderByDescending(x => x.ConcurrencyStamp);
            PagedList.Core.PagedList<RoleListViewModel> models = new PagedList.Core.PagedList<RoleListViewModel>(queryableResponse, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            //_toastNotification.AddSuccessToastMessage("Test Notification !");
            //_toastNotification.AddSuccessToastMessage("Woo hoo - it works!");

            //// Info Toast
            //_toastNotification.AddInfoToastMessage("Here is some information.");

            //// Error Toast
            //_toastNotification.AddErrorToastMessage("Woops an error occured.");

            //// Warning Toast
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

        [Route("Admin/AdminPermission/Create-Role")]
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
    }
}
