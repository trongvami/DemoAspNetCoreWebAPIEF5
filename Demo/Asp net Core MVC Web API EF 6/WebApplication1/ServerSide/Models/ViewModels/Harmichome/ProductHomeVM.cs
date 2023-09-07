using ServerSide.Entities;
using ServerSide.Models.ResponseModels;

namespace ServerSide.Models.ViewModels.Harmichome
{
    public class ProductHomeVM
    {
        public TbParent category { get; set; }
        public List<TbProduct> lsproducts { get; set; }
    }
}
