﻿namespace ServerSide.Models.ResponseModels
{
    public class UnitsListResponse
    {
        public string? UnitID { get; set; }
        public string? UnitName { get; set; }
        public bool IsActive { get; set; }
        public int? IsDelete { get; set; }
    }
}
