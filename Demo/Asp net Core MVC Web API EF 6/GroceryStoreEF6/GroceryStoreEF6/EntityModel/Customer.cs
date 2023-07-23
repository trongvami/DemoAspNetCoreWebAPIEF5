using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string Id { get; set; } = null!;
        /// <summary>
        /// Mật khẩu đăng nhập
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// Họ và tên
        /// </summary>
        public string Fullname { get; set; } = null!;
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// Hình
        /// </summary>
        public string Photo { get; set; } = null!;
        /// <summary>
        /// Đã kích hoạt hay chưa
        /// </summary>
        public bool Activated { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
