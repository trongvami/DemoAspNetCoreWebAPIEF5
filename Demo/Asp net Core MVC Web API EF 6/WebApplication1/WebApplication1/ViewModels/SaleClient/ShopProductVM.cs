using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.ResponseModels;

namespace WebApplication1.ViewModels.SaleClient
{
    public class ShopProductVM
    {
        public PagedList.Core.PagedList<ShopProduct> ShopProducts { get; set; }
        public List<ShopProduct> RelatedShopProducts { get; set; }
        public int OrderBy { get; set; }
        public List<SelectListItem> Levels { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Parents { get; set; }
    }
}
