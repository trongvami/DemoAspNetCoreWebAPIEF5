using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbLevel
    {
        public TbLevel()
        {
            TbCategories = new HashSet<TbCategory>();
            TbProducts = new HashSet<TbProduct>();
        }

        public int LevelCode { get; set; }
        public int? ParentId { get; set; }
        public string? LevelName { get; set; }
        public bool? LevelActive { get; set; }
        public int? LevelDelete { get; set; }

        public virtual TbParent? Parent { get; set; }
        public virtual ICollection<TbCategory> TbCategories { get; set; }
        public virtual ICollection<TbProduct> TbProducts { get; set; }
    }
}
