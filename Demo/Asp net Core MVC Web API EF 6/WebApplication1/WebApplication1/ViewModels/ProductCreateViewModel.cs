using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class ProductCreateViewModel
    {
        public ProductsListViewModel Product { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
        public IEnumerable<SelectListItem> Unitpays { get; set; }
    }
}
