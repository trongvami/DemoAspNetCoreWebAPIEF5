using System;

namespace MyFirstWebAPI.Models
{
    public class ProductModel
    {
        public Guid MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public double DonGia { get; set; }
        public string TenLoai { get; set; }
    }
}
