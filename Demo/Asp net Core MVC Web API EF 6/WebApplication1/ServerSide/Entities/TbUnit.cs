using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbUnit
    {
        public TbUnit()
        {
            TbProducts = new HashSet<TbProduct>();
        }

        public int UnitId { get; set; }
        public string? UnitName { get; set; }
        public bool? IsActive { get; set; }
        public int? IsDelete { get; set; }

        public virtual ICollection<TbProduct> TbProducts { get; set; }
    }
}
