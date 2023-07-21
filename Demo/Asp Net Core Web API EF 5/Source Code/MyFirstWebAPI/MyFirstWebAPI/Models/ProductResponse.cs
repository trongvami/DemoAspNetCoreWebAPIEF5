using System.ComponentModel.DataAnnotations;
using System;

namespace MyFirstWebAPI.Models
{
    public class ProductResponse
    {
        public Guid MaHH { get; set; }
        public string TenHH { get; set; }
        public string MoTa { get; set; }
        public double DonGia { get; set; }
        public byte Discount { get; set; }
        public int? MaLoai { get; set; }
    }
}
