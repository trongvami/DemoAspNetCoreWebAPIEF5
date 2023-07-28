using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using WebClient.Helpers;
using WebClient.ViewModels;
using Newtonsoft.Json;
using NuGet.Common;

namespace WebClient.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class RolePermissionController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpClientFactory _httpClientFactory;
        public RolePermissionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("Admin/RolePermission/Role-List")]
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            var token = Request.Cookies["IdentityToken"];

            // Nếu access token không tồn tại hoặc rỗng, có thể xử lý lỗi hoặc chuyển hướng đến trang đăng nhập
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            // Gửi yêu cầu GET đến API để lấy danh sách sản phẩm
            var apiGetAllRoleUrl = "https://localhost:7020/api/RolePermission/RoleList";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = JsonConvert.DeserializeObject<List<RoleListViewModel>>(httpClient.GetStringAsync(apiGetAllRoleUrl).Result);
            //var response = await HttpClientHelper.GetListAsync<RoleListViewModel>(apiGetAllRoleUrl, accessToken);
            //var response = await httpClient.GetAsync(apiGetAllRoleUrl);
            // Kiểm tra trạng thái của response
            if (response != null)
            {
                return View(response);
            }
            else
            {
                // Xử lý lỗi
                return View(response);
                // ...
            }
        }

        [Route("Admin/RolePermission/Create-Role")]
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }


    }
}
