using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbTransactStatus
    {
        public TbTransactStatus()
        {
            TbOrders = new HashSet<TbOrder>();
        }

        public int TransactStatusId { get; set; }
        public string Status { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
