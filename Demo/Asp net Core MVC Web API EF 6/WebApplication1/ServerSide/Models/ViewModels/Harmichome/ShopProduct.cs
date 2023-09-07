namespace ServerSide.Models.ViewModels.Harmichome
{
    public class ShopProduct
    {
        public int ProductId { get; set; }
        public int? CatId { get; set; }
        public int LevelCode { get; set; }
        public int ParentId { get; set; }
        public string? CatName { get; set; }
        public string LevelName { get; set; }
        public string ParentName { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? Thumb { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string? ShortDesc { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public int SoLuongConLai { get; set; }
        public string Alias { get; set; }
    }
}
