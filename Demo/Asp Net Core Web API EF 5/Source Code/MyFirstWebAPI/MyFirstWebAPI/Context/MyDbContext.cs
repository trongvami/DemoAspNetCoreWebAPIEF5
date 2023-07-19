using Microsoft.EntityFrameworkCore;
using MyFirstWebAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Order>(e=> {
                e.ToTable("Order");
                e.HasKey(o => o.MaDonHang);
                e.Property(o => o.NgayGiao).HasDefaultValueSql("getutcdate()");
                e.Property(o=>o.NguoiNhan).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetails>(entity => {
                entity.ToTable("OrderDetails");
                entity.HasKey(o => new { o.MaDonHang, o.MaHH });
                entity.HasOne(o => o.Order)
                        .WithMany(o => o.OrderDetails)
                        .HasForeignKey(o => o.MaDonHang)
                        .HasConstraintName("FK_OrderDetails_Order");
                entity.HasOne(o => o.Products)
                        .WithMany(o => o.OrderDetails)
                        .HasForeignKey(o => o.MaHH)
                        .HasConstraintName("FK_OrderDetails_Product");
            });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
