using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbShipper
    {
        public TbShipper()
        {
            TbOrders = new HashSet<TbOrder>();
        }

        public int ShipperId { get; set; }
        public string? ShipperName { get; set; }
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public DateTime? ShipDate { get; set; }

        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
