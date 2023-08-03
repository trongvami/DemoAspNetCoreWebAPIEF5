using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private static readonly HttpClient _httpClientt = new HttpClient();
        private readonly IToastNotification _toastNotification;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        public AdminAccountController(IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService)
        {
            _toastNotification = toastNotification;
            _notyfService = notyfService;
        }

        [Route("Admin/AdminAccount/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Admin/AdminAccount/Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                registerViewModel.Service = "Employee";
                var response = await HttpClientHelper.PostAsync("https://localhost:7071/api/Security/Register", registerViewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("RegisterSuccess", "AdminAccount", new { area = "Admin" });
                }
                return View(registerViewModel);
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await HttpClientHelper.PostAsync("https://localhost:7071/api/Security/login", loginViewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login2FA");
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        [Route("Admin/AdminAccount/Login-2FA")]
        public IActionResult Login2FA()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/AdminAccount/Login-2FA")]
        public async Task<IActionResult> Login2FA(LoginVerifyModel loginVerifyModel)
        {
            if (ModelState.IsValid)
            {
                var response = await HttpClientHelper.PostAsync("https://localhost:7071/api/Security/login-2FA", loginVerifyModel);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var tokenFromAPI = await response.Content.ReadAsStringAsync();
                        var responseData = JsonSerializer.Deserialize<JsonElement>(tokenFromAPI);
                        JObject jsonObject = JObject.Parse(tokenFromAPI);
                        string tokenValue = (string)jsonObject["token"];
                        var tokenElement = responseData.GetProperty("token");
                        //var expirationElement = responseData.GetProperty("expiration");
                        var userElement = responseData.GetProperty("user");

                        var claims = new List<Claim>();

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.ReadJwtToken(tokenValue);

                        //var token = new JwtSecurityToken(tokenFromAPI);

                        var userNameClaim = userElement;

                        claims.Add(new Claim(ClaimTypes.Name, userNameClaim.GetString()));
                        var roles = token.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => new Claim(ClaimTypes.Role, c.Value));
                        claims.AddRange(roles);

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        HttpContext.Response.Cookies.Append("IdentityToken", tokenValue.ToString(), new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(20),
                            HttpOnly = true,
                            Secure = true
                        });
                        _notyfService.Information("Login Successfully !", 4);
                        return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi đọc JWT token: " + ex.Message);
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid credentials.";
                    return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
                }
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }

            return View(loginVerifyModel);
        }

        [HttpGet]
        [Route("Admin/AdminAccount/Register-Success")]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/AdminAccount/LogOut")]
        public async Task<IActionResult> LogOut()
        {
            // Đăng xuất và hủy session và cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var response = await _httpClientt.GetAsync("https://localhost:7071/api/Security/logout");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
            }
            // Chuyển hướng về trang chủ hoặc trang đăng nhập
            return View();
        }
    }
}
