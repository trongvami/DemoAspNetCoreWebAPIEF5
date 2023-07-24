using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroceryStoreEF6.EntityModel;

namespace GroceryStoreEF6.Data
{
    public class GroceryStoreEF6Context : DbContext
    {
        public GroceryStoreEF6Context (DbContextOptions<GroceryStoreEF6Context> options)
            : base(options)
        {
        }

        public DbSet<GroceryStoreEF6.EntityModel.Category> Category { get; set; } = default!;
    }
}
