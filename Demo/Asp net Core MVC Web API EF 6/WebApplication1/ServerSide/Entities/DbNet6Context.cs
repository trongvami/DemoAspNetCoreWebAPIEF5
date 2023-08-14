using System;
using System.Collections.Generic;
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

        public virtual DbSet<TbAttribute> TbAttributes { get; set; } = null!;
        public virtual DbSet<TbAttributesPrice> TbAttributesPrices { get; set; } = null!;
        public virtual DbSet<TbCaLamViec> TbCaLamViecs { get; set; } = null!;
        public virtual DbSet<TbCategory> TbCategories { get; set; } = null!;
        public virtual DbSet<TbLocation> TbLocations { get; set; } = null!;
        public virtual DbSet<TbOrder> TbOrders { get; set; } = null!;
        public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; } = null!;
        public virtual DbSet<TbPage> TbPages { get; set; } = null!;
        public virtual DbSet<TbProduct> TbProducts { get; set; } = null!;
        public virtual DbSet<TbQuangCao> TbQuangCaos { get; set; } = null!;
        public virtual DbSet<TbShipper> TbShippers { get; set; } = null!;
        public virtual DbSet<TbTinDang> TbTinDangs { get; set; } = null!;
        public virtual DbSet<TbTransactStatus> TbTransactStatuses { get; set; } = null!;
        public virtual DbSet<TbUnit> TbUnits { get; set; } = null!;

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

            modelBuilder.Entity<TbAttribute>(entity =>
            {
                entity.HasKey(e => e.AttributeId);

                entity.Property(e => e.AttributeId).HasColumnName("AttributeID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TbAttributesPrice>(entity =>
            {
                entity.HasKey(e => e.AttributesPriceId);

                entity.Property(e => e.AttributesPriceId).HasColumnName("AttributesPriceID");

                entity.Property(e => e.AttributeId).HasColumnName("AttributeID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.TbAttributesPrices)
                    .HasForeignKey(d => d.AttributeId)
                    .HasConstraintName("FK_TbAttributesPrices_TbAttributes");
            });

            modelBuilder.Entity<TbCaLamViec>(entity =>
            {
                entity.HasKey(e => e.SoCaLamViec);

                entity.Property(e => e.SoCaLamViec).ValueGeneratedNever();

                entity.Property(e => e.MoTa).HasMaxLength(1000);
            });

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

            modelBuilder.Entity<TbLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameWithType).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.Type).HasMaxLength(10);
            });

            modelBuilder.Entity<TbOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentCreate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_TbOrders_TbLocations");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_TbOrders_TbShippers");

                entity.HasOne(d => d.TransactStatus)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.TransactStatusId)
                    .HasConstraintName("FK_TbOrders_TbTransactStatus");
            });

            modelBuilder.Entity<TbOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_TbOrderDetails_TbOrders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TbOrderDetails_TbProducts");
            });

            modelBuilder.Entity<TbPage>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MetaDesc).HasMaxLength(250);

                entity.Property(e => e.MetaKey).HasMaxLength(250);

                entity.Property(e => e.PageName).HasMaxLength(250);

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<TbProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Image1).HasMaxLength(1000);

                entity.Property(e => e.Image2).HasMaxLength(1000);

                entity.Property(e => e.Image3).HasMaxLength(1000);

                entity.Property(e => e.Image4).HasMaxLength(1000);

                entity.Property(e => e.Image5).HasMaxLength(1000);

                entity.Property(e => e.Image6).HasMaxLength(1000);

                entity.Property(e => e.MetaDesc).HasMaxLength(255);

                entity.Property(e => e.MetaKey).HasMaxLength(255);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.ShortDesc).HasMaxLength(255);

                entity.Property(e => e.Thumb).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.TbProducts)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_TbProducts_TbCategory");
            });

            modelBuilder.Entity<TbQuangCao>(entity =>
            {
                entity.HasKey(e => e.QuangCaoId);

                entity.Property(e => e.QuangCaoId).HasColumnName("QuangCaoID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImageBg)
                    .HasMaxLength(250)
                    .HasColumnName("ImageBG");

                entity.Property(e => e.ImageProduct).HasMaxLength(250);

                entity.Property(e => e.SubTitle).HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.Property(e => e.UrlLink).HasMaxLength(250);
            });

            modelBuilder.Entity<TbShipper>(entity =>
            {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.Company).HasMaxLength(150);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.ShipperName).HasMaxLength(150);
            });

            modelBuilder.Entity<TbTinDang>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.Author).HasMaxLength(255);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsHot).HasColumnName("isHot");

                entity.Property(e => e.IsNewfeed).HasColumnName("isNewfeed");

                entity.Property(e => e.MetaDesc).HasMaxLength(255);

                entity.Property(e => e.MetaKey).HasMaxLength(255);

                entity.Property(e => e.Scontents)
                    .HasMaxLength(255)
                    .HasColumnName("SContents");

                entity.Property(e => e.Thumb).HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<TbTransactStatus>(entity =>
            {
                entity.HasKey(e => e.TransactStatusId);

                entity.ToTable("TbTransactStatus");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<TbUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.UnitName).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
