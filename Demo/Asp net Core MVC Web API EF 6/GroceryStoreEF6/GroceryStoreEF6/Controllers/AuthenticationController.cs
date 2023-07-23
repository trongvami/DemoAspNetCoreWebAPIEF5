using GroceryStoreEF6.Model.ResponseModel;
using GroceryStoreEF6.Model.ViewModel.Authentication.SignUp;
using GroceryStoreEF6.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreEF6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role) { 
            // Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { Message = "User already existes !", Status = "Error" });
            }
            // Add the user in the database
            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "User Failed To Create !", Status = "Error" });
                }
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status201Created, new Response { Message = "User Created Successfully !", Status = "Success" });
            }
            else {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "This Role Doesn't Exist !", Status = "Error" });
            }

        }

        [HttpGet]
        public IActionResult TestEmail()
        {
            var message = new MessageResponse(new string[] 
                {"cuibapvcc@gmail.com"}, 
                "Test", 
                "<h1>Hello Trong, Try more and more</h1>"
            );
            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status200OK, new Response { Status="Success", Message="Email Sent Successfully!"});
        }
    }
}
