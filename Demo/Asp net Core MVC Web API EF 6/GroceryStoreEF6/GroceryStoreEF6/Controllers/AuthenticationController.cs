using GroceryStoreEF6.Common;
using GroceryStoreEF6.EntityModel;
using GroceryStoreEF6.Model.ResponseModel;
using GroceryStoreEF6.Model.ViewModel.Authentication.Login;
using GroceryStoreEF6.Model.ViewModel.Authentication.SignUp;
using GroceryStoreEF6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroceryStoreEF6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
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
                UserName = registerUser.UserName,
                TwoFactorEnabled = true
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "User Failed To Create !", Status = "Error" });
                }
                await _userManager.AddToRoleAsync(user, role);

                // add token to verify the email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                // Send mail verify code
                //T_VerifyCode verifyCode = new T_VerifyCode();
                //verifyCode.DonVi = nextMaDonVi;
                //verifyCode.CodeVerify = Utilities.RandomString(6);

                //await _verifyCodeRepository.AddAsync(verifyCode);

                string Attention1 = @"<p>We have received your request to register in our store.</p>";
                string Attention2 = @"<p>If you didn't request this code, you can safely ignore this email. Someone else might have typed your email address by mistake.</p>";
                string Signature = @"<p>Thanks,</p><p>The GroceryStore Team</p>";
                string Hi = String.Format(@"Hi {0},<br /> {1} <p>Please click here: <b>{2}</b></p> {3} {4}", user.UserName, Attention1, confirmationLink, Attention2, Signature);
                IEnumerable<string> lstEmail = new string[] { user.Email };
                MessageResponse messageModel = new MessageResponse(lstEmail, "GroceryStore - Link Confirmation to Create New Account", Hi);
                
                _emailService.SendEmail(messageModel);

                return StatusCode(StatusCodes.Status201Created, new Response { Message = $"User Created & Email Sent to {user.Email} Successfully !", Status = "Success" });
            }
            else {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "This Role Doesn't Exist !", Status = "Error" });
            }

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
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Email Verified Successfully!" });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status="Error", Message="This User doesn't exist!"});
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //checking the user
            var user = await _userManager.FindByEmailAsync(loginModel.Username);
            if (user.TwoFactorEnabled)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginModel.Password,false,true);
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var message = new MessageResponse(new string[] { user.Email! }, "GroceryStore - OTP Confirmation ", token);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"We have sent an OTP to your Email {user.Email}" });
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
                return Ok(new { 
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
                //generate the token with the claims

                //returning the token
            }

            return Unauthorized();
            
        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false,false);
            if (signIn.Succeeded)
            {
                if (user != null)
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
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                    //generate the token with the claims

                    //returning the token
                }
            }

            return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Success", Message = $"Invalid Code" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([Required] string email) { 
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new MessageResponse(new string[] { user.Email }, "Forgot Password Link", link!);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"Password changed request is sent on Email {user.Email}. Please Open Your Email and Click" });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"Couldn't send link to email. Please Try again" });
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new { model });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var rs = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!rs.Succeeded)
                {
                    foreach (var error in rs.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"Password has been changed" });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"Couldn't send link to email. Please Try again" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims:authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
