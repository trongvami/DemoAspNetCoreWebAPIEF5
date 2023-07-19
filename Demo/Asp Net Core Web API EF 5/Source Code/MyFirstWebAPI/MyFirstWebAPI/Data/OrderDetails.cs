using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Data
{
    public class OrderDetails
    {
        public Guid MaHH { get; set; }
        public Guid MaDonHang { get; set; }
        public int Amount { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        // Relationship
        public Order Order { get; set; }
        public Product Products { get; set; }
    }
}
