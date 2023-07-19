using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Data
{
    [Table("Product")]
    public class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public Guid MaHH { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenHH { get; set; }
        public string MoTa { get; set; }
        [Range(0, double.MaxValue)]
        public double DonGia { get; set; }
        public byte Discount { get; set; }
        public int? MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public Category Category { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
        
    }
}
