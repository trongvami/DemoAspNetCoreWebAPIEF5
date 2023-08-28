using ServerSide.Entities;

namespace WebApplication1.ViewModels.SaleClient
{
    public class ProductHomeVM
    {
        public TbParent category { get; set; }
        public List<TbProduct> lsproducts { get; set; }
    }
}
