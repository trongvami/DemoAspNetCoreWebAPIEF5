using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbUnitsPayment
    {
        public TbUnitsPayment()
        {
            TbProducts = new HashSet<TbProduct>();
        }

        public int UpayId { get; set; }
        public string? UpayName { get; set; }
        public bool? IsActive { get; set; }
        public int? IsDelete { get; set; }

        public virtual ICollection<TbProduct> TbProducts { get; set; }
    }
}
