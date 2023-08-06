using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerSide.MailServices;
using ServerSide.Models;
using ServerSide.Models.ResponseModels;
using ServerSide.Models.ViewModels.Authentication.Login;
using ServerSide.Models.ViewModels.Authentication.SignUp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public SecurityController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            // Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseBase { Message = "User already existes !", Status = "Error" });
            }
            // Add the user in the database
            var user = new ApplicationUser
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Email,
                TwoFactorEnabled = true,
                Firstname = registerUser.Firstname,
                Lastname = registerUser.Lastname,
                Middlename = registerUser.Middlename,
                Fullname = registerUser.Fullname,
                Service = registerUser.Service,
                Dob = registerUser.Dob,
                Address = registerUser.Address,
                PhoneNumber = registerUser.Phone
            };

            if (await _roleManager.RoleExistsAsync(registerUser.Service))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "User Failed To Create New Account !", Status = "Error" });
                }

                await _userManager.AddToRoleAsync(user, registerUser.Service);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Security", new { token, email = user.Email }, Request.Scheme);

                string Attention1 = @"<p>We have received your request to register in our store.</p>";
                string Attention2 = @"<p>If you didn't request this code, you can safely ignore this email. Someone else might have typed your email address by mistake.</p>";
                string Signature = @"<p>Thanks,</p><p>The GroceryStore Team</p>";
                string Hi = String.Format(@"Hi {0},<br /> {1} <p>Please click here: <b>{2}</b></p> {3} {4}", user.UserName, Attention1, confirmationLink, Attention2, Signature);
                IEnumerable<string> lstEmail = new string[] { user.Email };
                MessageResponse messageModel = new MessageResponse(lstEmail, "GroceryStore - Link Confirmation to Create New Account", Hi);

                _emailService.SendEmail(messageModel);

                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"User Created & Email Sent to {user.Email} Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "This Role Doesn't Exist !", Status = "Error" });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ExternalConfirmEmailRegisters")]
        public async Task<IActionResult> ExternalConfirmEmailRegisters([FromBody] string email)
        {
            // Check User Exist
            var userExist = await _userManager.FindByEmailAsync(email);
            if (userExist == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseBase { Message = "User doesn't exist !", Status = "Error" });
            }

            // Add the user in the database
            var user = new ApplicationUser
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = email,
                TwoFactorEnabled = true,
                Fullname = email,
                Service = "Employee"
            };

            await _userManager.AddToRoleAsync(user, user.Service);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Security", new { token, email = user.Email }, Request.Scheme);

            string Attention1 = @"<p>We have received your request to register in our store.</p>";
            string Attention2 = @"<p>If you didn't request this code, you can safely ignore this email. Someone else might have typed your email address by mistake.</p>";
            string Signature = @"<p>Thanks,</p><p>The GroceryStore Team</p>";
            string Hi = String.Format(@"Hi {0},<br /> {1} <p>Please click here: <b>{2}</b></p> {3} {4}", user.UserName, Attention1, confirmationLink, Attention2, Signature);
            IEnumerable<string> lstEmail = new string[] { user.Email };
            MessageResponse messageModel = new MessageResponse(lstEmail, "GroceryStore - Link Confirmation to Create New Account", Hi);

            _emailService.SendEmail(messageModel);

            return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"User Created & Email Sent to {user.Email} Successfully !", Status = "Success" });

        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var rs = await _userManager.ConfirmEmailAsync(user, token);
                if (rs.Succeeded)
                {
                    string htmlResponse = $@"
                        <html>
                        <head>
                            <style>
                              body {{
                margin: 0; 
                padding: 0; 
                height: 100vh;
                font-family: Arial, sans-serif;
            }}
            .container {{
                position: fixed;
                top: 40%;
                left: 50%;
                transform: translate(-50%, -50%);
                text-align: center;
                width: 80%; 
                max-width: 400px; 
                background-image: url('../adminassets/img/login.jpg'); /* Đường dẫn đến hình nền */
                background-size: cover; /* Hiển thị hình nền sao cho toàn bộ container được che phủ */
                background-position: center; /* Căn hình nền vào giữa container */
            }}
                                .success {{
                                    font-size: 18px;
                                    font-weight: bold;
                                    margin-top: 10px;
                                }}
                                .thankyou {{
                                    font-size: 15px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <p class='success'>Congratulation - Email Verified Successfully !</p>
                                <p class='success'>
                                    Thank you for confirming your email.
                                </p>
                            </div>
                        </body>
                        </html>";

                    return Content(htmlResponse, "text/html");
                    //return StatusCode(StatusCodes.Status200OK, new ResponseBase { Status = "Success", Message = "Email Verified Successfully!" });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Status = "Error", Message = "This User doesn't exist!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //checking the user
            var user = await _userManager.FindByEmailAsync(loginModel.Username);

            if (user.TwoFactorEnabled && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var message = new MessageResponse(new string[] { user.Email! }, "GroceryStore - OTP Confirmation ", token);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Status = "Success", Message = $"We have sent an OTP to your Email {user.Email}" });
            }
            //checking the password
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //claimlist creation
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //we add role to the list
                var jwtToken = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
                    //expiration = jwtToken.ValidTo,
                    //username = user.UserName
                });
                //generate the token with the claims

                //returning the token
            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> loginWithOTP([FromBody] LoginVerifyModel loginVerifyModel)
        {
            var user = await _userManager.FindByEmailAsync(loginVerifyModel.email.ToString());
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", loginVerifyModel.code.ToString(), false, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                    };

                    //claimlist creation
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    //we add role to the list
                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        //expiration = jwtToken.ValidTo,
                        user = user.UserName
                    });
                }
            }

            return StatusCode(StatusCodes.Status404NotFound, new ResponseBase { Status = "Success", Message = $"Invalid Code" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        public static string GenerateRandomAlphanumericString(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            return Ok();
        }

        [HttpGet]
        [Route("ExternalLogins")]
        public async Task<IActionResult> ExternalLogins()
        {
            var rs = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Ok(rs);
        }

        [HttpGet]
        [Route("GetExternalLoginInfor")]
        public async Task<IActionResult> GetExternalLoginInfor()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            return Ok(info);
        }

        [HttpGet]
        [Route("ConfigureExternalAuthentication/{provider}/{redirectUrl}")]
        public async Task<IActionResult> ConfigureExternalAuthentication(string provider, string redirectUrl)
        {
            var decodedRedirectUrl = WebUtility.UrlDecode(redirectUrl);
            var rs = _signInManager.ConfigureExternalAuthenticationProperties(provider, decodedRedirectUrl);
            return Ok(rs);
        }

        [HttpPost]
        [Route("ExternalLogInSignIn")]
        public async Task<IActionResult> ExternalLogInSignIn([FromBody] ExternalLoginVM externalLoginVM)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginVM.LoginProvider, externalLoginVM.ProviderKey, externalLoginVM.isPersistent, externalLoginVM.bypassTwoFactor);
            if (signInResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Status = "Success", Message = $"Login Successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseBase { Status = "Error", Message = $"Cannot found" });
            }
        }

        [HttpPost]
        [Route("AddLogInAndSignin")]
        public async Task<IActionResult> AddLogInAndSignin([FromBody] ExternalLoginInfo externalLoginInfo)
        {
            var user = await _userManager.FindByEmailAsync(externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    Service = "Employee",
                    Fullname = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email)
                };

                await _userManager.CreateAsync(user);
            }

            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Status = "Success", Message = $"Login Successfully" });
        }

    }
}
