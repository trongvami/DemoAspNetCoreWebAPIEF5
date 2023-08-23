namespace ServerSide.Models.ResponseModels
{
    public class LevelsListResponse
    {
        public string? LevelCode { get; set; }
        public string? ParentID { get; set; }
        public string? LevelName { get; set; }
        public bool LevelActive { get; set; }
        public int? LevelDelete { get; set; }
        public int? AmountCategory { get; set; }
        public int? AmountProduct { get; set; }
    }
}
