using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Persistent
{
    public class DbNet6Context : IdentityDbContext<ApplicationUser>
    {
        public DbNet6Context()
        {
        }

        public DbNet6Context(DbContextOptions<DbNet6Context> options)
            : base(options)
        {
        }
    }
}
