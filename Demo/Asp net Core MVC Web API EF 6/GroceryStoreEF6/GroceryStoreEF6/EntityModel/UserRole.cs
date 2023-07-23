using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string RoleId { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
