using System.Security.Claims;

namespace WebApplication1.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create Role","True"),
            new Claim("Edit Role","True"),
            new Claim("Delete Role","True"),

            new Claim("Create Employee","True"),
            new Claim("Update Employee","True"),
            new Claim("Delete Employee","True"),

            new Claim("Create Customer","True"),
            new Claim("Update Customer","True"),
            new Claim("Delete Customer","True"),

            new Claim("Create Claim","True"),
            new Claim("Update Claim","True"),
            new Claim("Delete Claim","True")
        };
    }
}
