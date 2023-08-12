using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbLocation
    {
        public TbLocation()
        {
            TbOrders = new HashSet<TbOrder>();
        }

        public int LocationId { get; set; }
        public string? Name { get; set; }
        public int? Parent { get; set; }
        public int? Levels { get; set; }
        public string? Slug { get; set; }
        public string? NameWithType { get; set; }
        public string? Type { get; set; }

        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
