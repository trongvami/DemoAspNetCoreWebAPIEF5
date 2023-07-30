using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServerSide.Models;

namespace ServerSide.Entities
{
    public partial class DbNet6Context : IdentityDbContext<ApplicationUser>
    {
        public DbNet6Context()
        {
        }

        public DbNet6Context(DbContextOptions<DbNet6Context> options)
            : base(options)
        {
        }

        public virtual DbSet<TbCategory> TbCategories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=DemoFullIdentityNet6;Persist Security Info=True;User ID=sa;Password=sa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //SeedRoles(modelBuilder);
            modelBuilder.Entity<TbCategory>(entity =>
            {
                entity.HasKey(e => e.CatId);

                entity.ToTable("TbCategory");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Alias).HasMaxLength(250);

                entity.Property(e => e.CatName).HasMaxLength(250);

                entity.Property(e => e.Cover).HasMaxLength(255);

                entity.Property(e => e.MetaDesc).HasMaxLength(250);

                entity.Property(e => e.MetaKey).HasMaxLength(250);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ShortContent).HasMaxLength(250);

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //private static void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityRole>().HasData(
        //            new IdentityRole() { Name = "Customer", ConcurrencyStamp = "1", NormalizedName = "Customer" },
        //            new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
        //            new IdentityRole() { Name = "Admin", ConcurrencyStamp = "3", NormalizedName = "Admin" },
        //            new IdentityRole() { Name = "Manager", ConcurrencyStamp = "4", NormalizedName = "Manager" },
        //            new IdentityRole() { Name = "Leader", ConcurrencyStamp = "5", NormalizedName = "Leader" }
        //        );
        //}
    }
}
