using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Id { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
