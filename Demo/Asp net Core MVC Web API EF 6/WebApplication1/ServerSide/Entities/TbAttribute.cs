using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbAttribute
    {
        public TbAttribute()
        {
            TbAttributesPrices = new HashSet<TbAttributesPrice>();
        }

        public int AttributeId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TbAttributesPrice> TbAttributesPrices { get; set; }
    }
}
