using System;
using System.Collections.Generic;

namespace ServerSide.Entities
{
    public partial class TbCaLamViec
    {
        public int SoCaLamViec { get; set; }
        public int MaCaLamViec { get; set; }
        public string? MoTa { get; set; }
        public double? HeSoCaLamViec { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
