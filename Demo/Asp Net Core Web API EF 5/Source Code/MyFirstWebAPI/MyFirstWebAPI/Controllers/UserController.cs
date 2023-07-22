using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFirstWebAPI.Context;
using MyFirstWebAPI.Data;
using MyFirstWebAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;
        private readonly AppSettings _appSettings;

        public UserController(MyDbContext myDbContext, IOptionsMonitor<AppSettings> optionsMonitor) {
            _myDbContext = myDbContext;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel loginModel)
        {
            var user = _myDbContext.NguoiDungs.SingleOrDefault(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password);
            if (user == null) // ko dung
            {
                return Ok(new APIResponse
                {
                    Success = false,
                    Message = "Invalid UserName/Password"
                });
            }
            // cap token
            else { 
                return Ok(new APIResponse { 
                    Success = true,
                    Message = "Authenticate Successfully",
                    Data = GenerateToken(user)
                });
            }
        }

        private string GenerateToken(NguoiDung nguoiDung) {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(ClaimTypes.Email, nguoiDung.Email),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),
                    // roles
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
