using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Data
{
    public enum TinhTrangDonHang
    {
        New = 0,
        Payment = 1,
        Complete = 2,
        Cancel = 3
    }

    public class Order
    {
        public Guid MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public TinhTrangDonHang TinhTrangDonHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChi { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        public Order() {
            OrderDetails = new List<OrderDetails>();
        }
    }
}
