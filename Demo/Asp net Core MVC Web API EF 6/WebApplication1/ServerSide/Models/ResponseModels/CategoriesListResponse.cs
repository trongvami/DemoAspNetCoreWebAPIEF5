namespace ServerSide.Models.ResponseModels
{
    public class CategoriesListResponse
    {
        public string? CatId { get; set; }
        public string CatName { get; set; }
        public string? ShortContent { get; set; }
        public string? Description { get; set; }
        public int? ParentID { get; set; }
        public int? Levels { get; set; }
        public int? Ordering { get; set; }
        public string? Thumb { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public bool Published { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public string? SchemaMarkup { get; set; }
    }
}
