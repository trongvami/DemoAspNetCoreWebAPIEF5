using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models;
using ServerSide.Models.ResponseModels;
using System.Data;
using System.Net.Http;
using System.Security.Claims;

namespace ServerSide.Controllers
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

        #region Role

        [HttpGet]
        [Authorize]
        [Route("RoleList")]
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            List<RoleListResponse> rolesList = new List<RoleListResponse>();
            foreach (var role in roles)
            {
                RoleListResponse item = new RoleListResponse();
                item.Id = role.Id;
                item.Name = role.Name;
                item.NormalizedName = role.NormalizedName;
                //item.ConcurrencyStamp = role.ConcurrencyStamp.ToString();
                rolesList.Add(item);
            }
            return Ok(rolesList);
        }

        [HttpGet]
        [Authorize]
        [Route("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            RoleListResponse item = new RoleListResponse();
            item.Id = role.Id;
            item.Name = role.Name;
            item.NormalizedName = role.NormalizedName;
            //item.ConcurrencyStamp = role.ConcurrencyStamp;
            return Ok(item);
        }

        [HttpPost]
        [Route("AddRole")]
        [Authorize]
        public async Task<IActionResult> AddNewRole([FromBody] RoleListResponse roleListResponse)
        {
            var role = await _roleManager.Roles.Where(x => x.Name == roleListResponse.Name).ToListAsync();
            if (role.Count > 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseBase { Message = "This Role already exists", Status = "Error" });
            }

            IdentityRole identityRole = new IdentityRole();
            identityRole.Name = roleListResponse.Name;
            identityRole.NormalizedName = roleListResponse.Name;
            //identityRole.ConcurrencyStamp = roleListResponse.ConcurrencyStamp;

            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = "Create New Role Successfully", Status = "Success" });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" }); ;
        }

        [HttpPut]
        [Route("UpdateRole")]
        [Authorize]
        public async Task<IActionResult> UpdateRole([FromBody] RoleListResponse roleListResponse)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == roleListResponse.Id).FirstOrDefaultAsync();
            if (role == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseBase { Message = "This Role couldn't find", Status = "Error" });
            }

            IdentityRole identityRole = new IdentityRole();
            identityRole.Name = roleListResponse.Name;
            identityRole.NormalizedName = roleListResponse.Name;
            //identityRole.ConcurrencyStamp = roleListResponse.ConcurrencyStamp;

            role.Name = identityRole.Name;
            role.NormalizedName = identityRole.Name;
            //role.ConcurrencyStamp = identityRole.ConcurrencyStamp;

            IdentityResult result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Update Role Successfully", Status = "Success" });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" }); ;
        }

        [HttpDelete]
        [Route("DeleteRole/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRole(string? id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var rs = await _roleManager.DeleteAsync(role);
            if (rs.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Delete Role Successfully", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" }); ;
            }
        }

        #endregion


        #region Claims
        [HttpGet]
        [Route("GetClaimsByRoleId/{id}")]
        public async Task<IActionResult> GetClaimsByRoleId(string? id)
        {
            var claimList = ClaimsStore.AllClaims.ToList();

            var roleFirst = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);

            var listClaimsResponse = new List<ClaimsResponse>();
            var listClaimsByRole = await _roleManager.GetClaimsAsync(roleFirst);
            foreach (var claim in claimList)
            {
                var claimsResponse = new ClaimsResponse {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };

                if (listClaimsByRole.Count == 0)
                {
                    claimsResponse.IsSelected = false;
                }
                else {
                    var matchingClaims = listClaimsByRole.Where(c => c.Type == claim.Type);
                    if (matchingClaims.Any())
                    {
                        claimsResponse.IsSelected = true;
                    }
                }
                listClaimsResponse.Add(claimsResponse);
            }

            var model = new RoleClaimsResponse();
            model.listClaims = listClaimsResponse;
            model.roleId = roleFirst.Id;
            var rs = await _roleManager.Roles.ToListAsync();
            var rsRLR = new List<RoleListResponse>();
            foreach (var r in rs)
            {
                var row = new RoleListResponse { 
                    Id = r.Id,
                    Name = r.Name,
                    NormalizedName = r.NormalizedName
                };
                rsRLR.Add(row);
            }
            model.listRoles = rsRLR;

            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateClaims")]
        [Authorize]
        public async Task<IActionResult> UpdateClaims([FromBody] ClaimsModelUpdate claimsModelUpdate)
        {
            string roleId = claimsModelUpdate.roleId;
            List<string> stringList = claimsModelUpdate.claimsList;

            var claimList = ClaimsStore.AllClaims.ToList();

            var role = await _roleManager.Roles.SingleOrDefaultAsync(x=>x.Id == roleId);
            var listClaimsByRole = await _roleManager.GetClaimsAsync(role);
            bool checkk = true;
            foreach (var item in stringList)
            {
                var temp = claimList.SingleOrDefault(x=>x.Type == item);
                if (listClaimsByRole.Count == 0)
                {
                    var status = await _roleManager.AddClaimAsync(role, temp);
                    if (!status.Succeeded) {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" });
                    }
                }
                else {
                    var matchingClaims = listClaimsByRole.Where(c => c.Type == item);
                    if (!matchingClaims.Any()) {
                        var status = await _roleManager.AddClaimAsync(role, temp);
                        if (!status.Succeeded)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" });
                        }
                    }
                }
            }

            foreach (var itemDb in listClaimsByRole)
            {
                var temp = claimList.SingleOrDefault(x => x.Type == itemDb.Type);
                var matchingClaims = stringList.Where(c => c == itemDb.Type);
                if (!matchingClaims.Any()) {
                    var status = await _roleManager.RemoveClaimAsync(role, temp);
                    if (!status.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Something is wrong", Status = "Error" });
                    }
                }
            }

            return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = "Add New Claims Successfully", Status = "Success" });
        }
        #endregion
    }
}
