using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Mô tả đơn vị tính
        /// </summary>
        public string UnitBrief { get; set; } = null!;
        /// <summary>
        /// Đơn giá
        /// </summary>
        public double UnitPrice { get; set; }
        /// <summary>
        /// Hình ảnh
        /// </summary>
        public string Image { get; set; } = null!;
        /// <summary>
        /// Ngày sản xuất
        /// </summary>
        public DateTime ProductDate { get; set; }
        public bool? Available { get; set; }
        /// <summary>
        /// Mô tả hàng hóa
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Mã loại, FK
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public string SupplierId { get; set; } = null!;
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public bool Special { get; set; }
        public bool Latest { get; set; }
        public int Views { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
