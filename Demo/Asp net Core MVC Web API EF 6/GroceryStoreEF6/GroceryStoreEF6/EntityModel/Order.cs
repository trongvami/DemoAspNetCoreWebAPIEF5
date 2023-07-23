using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        /// <summary>
        /// Mã hóa đơn
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CustomerId { get; set; } = null!;
        /// <summary>
        /// Ngày đặt hàng
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Ngày cần có hàng
        /// </summary>
        public DateTime RequireDate { get; set; }
        /// <summary>
        /// Tên người nhận
        /// </summary>
        public string Receiver { get; set; } = null!;
        /// <summary>
        /// Địa chỉ nhận
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// Ghi chú thêm
        /// </summary>
        public string? Description { get; set; }
        public double Amount { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
