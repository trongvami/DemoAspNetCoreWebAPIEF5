using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Models
{
    public class HangHoa
    {
        [Key]
        public Guid HangHoaId { get; set; }
        public string TenHangHoa { get; set; }
        public int DonGia { get; set; }
    }
}
