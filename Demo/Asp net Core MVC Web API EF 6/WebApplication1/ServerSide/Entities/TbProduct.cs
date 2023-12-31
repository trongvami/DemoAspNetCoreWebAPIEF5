﻿using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbProduct
    {
        public TbProduct()
        {
            TbOrderDetails = new HashSet<TbOrderDetail>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDesc { get; set; }
        public string? Description { get; set; }
        public int? CatId { get; set; }
        public int? LevelCode { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string? Thumb { get; set; }
        public string? Video { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? BestSellers { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? Active { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public int? SoLuongBanDau { get; set; }
        public int? SoLuongDaBan { get; set; }
        public int? UnitsInStock { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public string? Image6 { get; set; }
        public int? UnitId { get; set; }
        public int? UpayId { get; set; }
        public int? Height { get; set; }

        public virtual TbCategory? Cat { get; set; }
        public virtual TbLevel? LevelCodeNavigation { get; set; }
        public virtual TbUnit? Unit { get; set; }
        public virtual TbUnitsPayment? Upay { get; set; }
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
    }
}
