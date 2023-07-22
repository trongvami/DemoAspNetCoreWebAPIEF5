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
using System.Security.Cryptography;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Validate(LoginModel loginModel)
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
                var token = await GenerateToken(user);
                return Ok(new APIResponse { 
                    Success = true,
                    Message = "Authenticate Successfully",
                    Data = token
                });
            }
        }

        private async Task<TokenModel> GenerateToken(NguoiDung nguoiDung) {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),
                    // roles
                    //new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            // save into database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = nguoiDung.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            await _myDbContext.AddAsync(refreshTokenEntity);
            await _myDbContext.SaveChangesAsync();

            return new TokenModel { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public static string GenerateRefreshToken() { 
            var randomStr = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomStr);
            }
            return Convert.ToBase64String(randomStr);
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenModel) {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenParamValidation = new TokenValidationParameters
            {
                // tu cap token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false // ko kiem tra token het han
            };

            try
            {
                // check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenParamValidation, out var validatedToken);

                // check 2: Check Alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken) {
                    var rs = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!rs) // false
                    {
                        return Ok(new APIResponse { 
                            Success = false,
                            Message = "Invalid Token"
                        });
                    }
                }

                // check 3: Check AccessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow) {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Access Token hasn't expired yet !"
                    });
                }

                // check 4: Check RefreshToken exist in db
                var storedToken = _myDbContext.RefreshTokens.FirstOrDefault(x=>x.Token == tokenModel.RefreshToken);

                if (storedToken == null)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Refresh Token doesn't exists!"
                    });
                }

                // Check 5: Check refresh token is used?
                if (storedToken.IsUsed) {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Refresh Token has been used!"
                    });
                }

                // Check 6: Check refresh token is revoked?
                if (storedToken.IsRevoked)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Refresh Token has been revoked!"
                    });
                }

                // Check 7: AccessTokenId == jwtId in refreshtoken
                var jti = tokenInVerification.Claims.FirstOrDefault(x=>x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (jti != storedToken.JwtId) {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Token doesn't match!"
                    });
                }

                // Check 8: Update Token Is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _myDbContext.Update(storedToken);
                await _myDbContext.SaveChangesAsync();

                // create new token
                var user =  await _myDbContext.NguoiDungs.SingleOrDefaultAsync(nd=>nd.Id == storedToken.UserId);
                var token = await GenerateToken(user);
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "Refresh Token Successfully",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0,0,0,0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
