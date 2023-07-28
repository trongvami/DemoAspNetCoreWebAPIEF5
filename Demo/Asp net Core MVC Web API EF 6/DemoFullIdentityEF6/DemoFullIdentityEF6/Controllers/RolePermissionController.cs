using DemoFullIdentityEF6.Data;
using DemoFullIdentityEF6.Data.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DemoFullIdentityEF6.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolePermissionController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles ="User")]
        [Route("RoleList")]
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<RoleListResponse> rolesList = new List<RoleListResponse>();
            foreach (var role in roles) { 
                RoleListResponse item = new RoleListResponse();
                item.Id = role.Id;
                item.Name = role.Name;
                item.NormalizedName = role.NormalizedName;
                item.ConcurrencyStamp = role.ConcurrencyStamp;
                rolesList.Add(item);
            }
            return Ok(rolesList);
        }
    }
}
