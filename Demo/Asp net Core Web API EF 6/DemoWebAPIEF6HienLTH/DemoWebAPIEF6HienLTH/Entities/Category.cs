using System;
using System.Collections.Generic;

namespace DemoWebAPIEF6HienLTH.Entities
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
        public string? Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
