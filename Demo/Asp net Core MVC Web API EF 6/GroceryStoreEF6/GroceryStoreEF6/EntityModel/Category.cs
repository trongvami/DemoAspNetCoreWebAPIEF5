using System;
using System.Collections.Generic;

namespace GroceryStoreEF6.EntityModel
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// Mã loại
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên chủng loại
        /// </summary>
        public string NameVn { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
