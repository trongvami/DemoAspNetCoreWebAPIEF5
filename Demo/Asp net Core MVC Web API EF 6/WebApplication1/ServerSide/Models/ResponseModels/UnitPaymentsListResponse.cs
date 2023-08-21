namespace ServerSide.Models.ResponseModels
{
    public class UnitPaymentsListResponse
    {
        public string? UpayId { get; set; }
        public string? UpayName { get; set; }
        public bool IsActive { get; set; }
        public int? IsDelete { get; set; }
    }
}
