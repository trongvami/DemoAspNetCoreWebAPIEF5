using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebClient.Helpers;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            // Gửi yêu cầu đăng nhập đến Web API
            var response = await HttpClientHelper.PostAsync("https://your-auth-server-url/api/auth/login", loginViewModel);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                // Lưu token vào cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Authentication, token) }, CookieAuthenticationDefaults.AuthenticationScheme)));

                // Kiểm tra vai trò của người dùng và chuyển hướng tới trang tương ứng
                var roles = await HttpClientHelper.GetAsync<List<string>>("https://your-auth-server-url/api/auth/user-roles");
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("AdminData");
                }
                else if (roles.Contains("User"))
                {
                    return RedirectToAction("SecureData");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials.";
                return View(loginViewModel);
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
