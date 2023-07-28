using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using WebClient.Helpers;
using WebClient.ViewModels;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IDistributedCache _cache;
        private static readonly HttpClient _httpClientt = new HttpClient();
        public AccountController(IDistributedCache cache) {
            _cache = cache;
        }

        [Route("Admin/Account/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Admin/Account/Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) {
                registerViewModel.Service = "User";
                var response = await HttpClientHelper.PostAsync("https://localhost:7020/api/Authentication/Register", registerViewModel);
                if (response.IsSuccessStatusCode) {
                    return RedirectToAction("RegisterSuccess", "Account", new { area = "Admin" });
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
            if (ModelState.IsValid) {
                var response = await HttpClientHelper.PostAsync("https://localhost:7020/api/Authentication/login", loginViewModel);
                if (response.IsSuccessStatusCode) {
                    return RedirectToAction("Login2FA");
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        [Route("Admin/Account/Login-2FA")]
        public IActionResult Login2FA()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/Account/Login-2FA")]
        public async Task<IActionResult> Login2FA(LoginVerifyModel loginVerifyModel)
        {
            if (ModelState.IsValid) {
                var response = await HttpClientHelper.PostAsync("https://localhost:7020/api/Authentication/login-2FA", loginVerifyModel);

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

                        HttpContext.Response.Cookies.Append("IdentityToken",tokenValue.ToString(), new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(10), // Thời gian hết hạn của cookie
                            HttpOnly = true,
                            Secure = true // Yêu cầu sử dụng HTTPS
                        });

                        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Authentication, tokenFromAPI) }, CookieAuthenticationDefaults.AuthenticationScheme)));

                        //if (roles.Contains("Admin"))
                        //{
                        //    return RedirectToAction("AdminData");
                        //}
                        //else if (roles.Contains("User"))
                        //{
                        //    return RedirectToAction("SecureData");
                        //}

                        //var tokenValue = tokenElement.GetString();
                        //var expirationValue = expirationElement.GetDateTime();
                        //var userValue = userElement.GetString();
                        //HttpContext.Session.SetString("AuthToken", tokenValue);
                        //HttpContext.Session.SetString("AuthExpiration", expirationValue.ToString());
                        //HttpContext.Session.SetString("UserName", userValue);
                        //string userValues = HttpContext.Session.GetString("UserName");

                        //ViewData["UserName"] = userValues;

                        //var cacheEntryOptions = new DistributedCacheEntryOptions
                        //{
                        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Thời gian sống của session (ví dụ 30 phút)
                        //};


                        //await _cache.SetStringAsync("UserName", userValues, cacheEntryOptions);
                        //await _cache.SetStringAsync("AuthExpiration", expirationValue.ToString(), cacheEntryOptions);
                        //await _cache.SetStringAsync("AuthToken", tokenValue, cacheEntryOptions);
                        //await _cache.SetStringAsync("tokenElement", tokenElement.ToString(), cacheEntryOptions);
                        //await _cache.SetStringAsync("tokenSendBackServer", tokenFromAPI, cacheEntryOptions);

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ tại đây và xem thông báo lỗi
                        Console.WriteLine("Lỗi khi đọc JWT token: " + ex.Message);
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid credentials.";
                    return RedirectToAction("Login", "Account", new { area = "Admin" });
                }
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            return View(loginVerifyModel);
            

            //var response = await HttpClientHelper.PostAsync("https://localhost:7020/api/Authentication/login-2FA", loginVerifyModel);

            //if (response.IsSuccessStatusCode)
            //{
            //    var tokenFromAPI = await response.Content.ReadAsStringAsync();


            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var token = tokenHandler.ReadJwtToken(tokenFromAPI);


            //    var roles = token.Claims.Where(c => c.Type == ClaimTypes.Role)
            //                            .Select(c => new Claim(ClaimTypes.Role, c.Value));

            //    var firstName = token.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            //    var middleName = token.Claims.FirstOrDefault(c => c.Type == "MiddleName")?.Value;
            //    var lastName = token.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            //    var fullName = $"{firstName} {middleName} {lastName}".Trim();
            //    var address = token.Claims.FirstOrDefault(c => c.Type == "Address")?.Value;


            //    var claims = new List<Claim>();
            //    claims.AddRange(roles);
            //    claims.Add(new Claim(ClaimTypes.Name, fullName));
            //    claims.Add(new Claim("Address", address));

            //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var principal = new ClaimsPrincipal(identity);

            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //    return RedirectToAction("Index", "Home", new { area = "Admin" });
            //}
            //else
            //{
            //    ViewBag.ErrorMessage = "Invalid credentials.";
            //    return View(loginVerifyModel);
            //}
        }

        [HttpGet]
        [Route("Admin/Account/Register-Success")]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Account/LogOut")]
        public async Task<IActionResult> LogOut() {
            // Đăng xuất và hủy session và cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var response = await _httpClientt.GetAsync("https://localhost:7020/api/Authentication/logout");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            // Chuyển hướng về trang chủ hoặc trang đăng nhập
            return View();
        }
    }
}
