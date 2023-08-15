namespace ServerSide.Models.ResponseModels
{
    public class ParentsListResponse
    {
        public string? ParentID { get; set; }
        public string ParentName { get; set; }
        public bool? ParentActive { get; set; }
        public int? ParentDelete { get; set; }
    }
}
