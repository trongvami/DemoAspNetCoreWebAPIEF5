using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Data
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int MaLoai { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenLoai { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
