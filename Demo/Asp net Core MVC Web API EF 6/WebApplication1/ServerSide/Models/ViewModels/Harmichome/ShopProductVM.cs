using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.ResponseModels;

namespace ServerSide.Models.ViewModels.Harmichome
{
    public class ShopProductVM
    {
        public List<ShopProduct> ShopProducts { get; set; }
        public List<ShopProduct> RelatedShopProducts { get; set; }
        public int OrderBy { get; set; }
        public List<LevelsListResponse> Levels { get; set; }
        public List<CategoriesListResponse> Categories { get; set; }
        public List<ParentsListResponse> Parents { get; set; }
    }
}
