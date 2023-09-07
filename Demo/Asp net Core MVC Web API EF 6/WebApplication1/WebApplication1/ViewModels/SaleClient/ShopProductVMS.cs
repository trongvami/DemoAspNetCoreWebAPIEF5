using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels.SaleClient
{
    public class ShopProductVMS
    {
        public List<ShopProduct> ShopProducts { get; set; }
        public List<ShopProduct> RelatedShopProducts { get; set; }
        public int OrderBy { get; set; }
        public List<LevelsListViewModel> Levels { get; set; }
        public List<CategoriesListViewModel> Categories { get; set; }
        public List<ParentsListViewModel> Parents { get; set; }
    }
}
