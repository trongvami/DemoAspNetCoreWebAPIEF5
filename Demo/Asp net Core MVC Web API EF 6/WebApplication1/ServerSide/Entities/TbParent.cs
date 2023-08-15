using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbParent
    {
        public TbParent()
        {
            TbCategories = new HashSet<TbCategory>();
            TbLevels = new HashSet<TbLevel>();
        }

        public int ParentId { get; set; }
        public string? ParentName { get; set; }
        public bool? ParentActive { get; set; }
        public int? ParentDelete { get; set; }

        public virtual ICollection<TbCategory> TbCategories { get; set; }
        public virtual ICollection<TbLevel> TbLevels { get; set; }
    }
}
