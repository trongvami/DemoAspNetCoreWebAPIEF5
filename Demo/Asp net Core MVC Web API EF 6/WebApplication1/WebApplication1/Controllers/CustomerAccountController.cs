using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class CustomerAccountController : Controller
    {
        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult ShoppingCart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
