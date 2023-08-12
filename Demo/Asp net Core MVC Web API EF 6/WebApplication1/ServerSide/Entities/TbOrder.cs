using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbOrder
    {
        public TbOrder()
        {
            TbOrderDetails = new HashSet<TbOrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? TransactStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? TotalMoney { get; set; }
        public int? PaymentId { get; set; }
        public string? Note { get; set; }
        public string? Address { get; set; }
        public int? LocationId { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public DateTime? PaymentCreate { get; set; }
        public int? OriginalMoney { get; set; }
        public int? DiscountMoney { get; set; }
        public int? ShipperId { get; set; }

        public virtual TbLocation? Location { get; set; }
        public virtual TbShipper? Shipper { get; set; }
        public virtual TbTransactStatus? TransactStatus { get; set; }
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
    }
}
