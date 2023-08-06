using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        public string? Service { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
    }
}
