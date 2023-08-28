using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class ProductsViewModel
    {
        public PagedList.Core.IPagedList<WebApplication1.ViewModels.ProductsListViewModel2> lsProduct { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Parents { get; set; }
    }
}
