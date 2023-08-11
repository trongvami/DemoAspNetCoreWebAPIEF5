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
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static readonly HttpClient _httpClientt = new HttpClient();
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AspNetCoreHero.ToastNotification.Abstractions.INotyfService _notyfService { get; set; }
        public AdminAccountController(IToastNotification toastNotification, AspNetCoreHero.ToastNotification.Abstractions.INotyfService notyfService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _toastNotification = toastNotification;
            _notyfService = notyfService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Login(string? returnUrl)
        {
            var apiUrl = "https://localhost:7071/api/Security/ExternalLogins";
            var res = JsonConvert.DeserializeObject<List<AuthenticationScheme>>(_httpClientt.GetStringAsync(apiUrl).Result);
            LoginViewModel model = new LoginViewModel {
                ReturnUrl = returnUrl,
                ExternalLogins = res
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var apiUrl = "https://localhost:7071/api/Security/ConfigureExternalAuthentication/";
            var redirectUrl = Url.Action("ExternalLoginCallback", "AdminAccount", new { ReturnUrl = returnUrl });
            var encodedRedirectUrl = WebUtility.UrlEncode(redirectUrl);
            var apiUrlWithParams = apiUrl + provider + "/" + encodedRedirectUrl;
            var res = JsonConvert.DeserializeObject<AuthenticationProperties>(_httpClientt.GetStringAsync(apiUrlWithParams).Result);
            return new ChallengeResult(provider, res);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError)
        {
            var apiUrl = "https://localhost:7071/api/Security/ExternalLogins";
            var res = JsonConvert.DeserializeObject<List<AuthenticationScheme>>(_httpClientt.GetStringAsync(apiUrl).Result);
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel {
                ReturnUrl = returnUrl,
                ExternalLogins = res
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            var apiGetExternalLoginInfoUrl = "https://localhost:7071/api/Security/GetExternalLoginInfor";
            var info2 = JsonConvert.DeserializeObject<ExternalLoginInfo>(_httpClientt.GetStringAsync(apiGetExternalLoginInfoUrl).Result);
            if (info == null)
            {
                ModelState.AddModelError(string.Empty,"Error loading external login information");
                return View("Login",loginViewModel);
            }

            var tempLoginProviders = info.LoginProvider;
            var tempProviderKeys = info.ProviderKey;
            bool isPersistents = false;
            bool bypassTwoFactors = true;

            ExternalLoginModel externalLoginModel = new ExternalLoginModel { 
                LoginProvider = tempLoginProviders,
                ProviderKey = tempProviderKeys,
                isPersistent = isPersistents,
                bypassTwoFactor = bypassTwoFactors
            };

            var apiSignInUrl = "https://localhost:7071/api/Security/ExternalLogInSignIn";
            var result = await HttpClientHelper.PostAsync(apiSignInUrl, externalLoginModel);

            if (result.IsSuccessStatusCode)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                LoginVerifyModel model = new LoginVerifyModel
                {
                    email = email,
                    code = "Exter1234"
                };

                var response = await HttpClientHelper.PostAsync("https://localhost:7071/api/Security/login-2FA", model);
                var tokenFromAPI = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(tokenFromAPI);
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
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    AddLoginUserManager addLoginUserManager = new AddLoginUserManager { 
                        email = email,
                        ExternalLoginInfo = info
                    };
                    //var apiAddLoginUserManagerAndSignInUrl = "https://localhost:7071/api/Security/AddLogInAndSignin";
                    //var data = await HttpClientHelper.Post2Async(apiAddLoginUserManagerAndSignInUrl, info);

                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            Service = "Employee",
                            Fullname = email,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            TwoFactorEnabled = true,
                            EmailConfirmed = true
                        };
                        var status1 = await _userManager.CreateAsync(user,"Exter1234");
                        var status2 = await _userManager.AddToRoleAsync(user, user.Service);
                    }

                    await _userManager.AddLoginAsync(user, info);

                    LoginViewModel model = new LoginViewModel
                    {
                        UserName = email,
                        Password = "Exter1234"
                    };

                    var response = await HttpClientHelper.PostAsync("https://localhost:7071/api/Security/login", model);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login2FA");
                    }
                }
                _notyfService.Error($"Email claim not received from: {info.LoginProvider}");
                _notyfService.Error($"Please contact support on ndtrong1803@gmail.com");
                return View("Error");
            }
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
                        var responseData = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(tokenFromAPI);
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
