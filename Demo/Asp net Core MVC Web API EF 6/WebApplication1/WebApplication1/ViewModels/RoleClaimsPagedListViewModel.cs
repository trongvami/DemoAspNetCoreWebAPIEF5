using PagedList.Core;

namespace WebApplication1.ViewModels
{
    public class RoleClaimsPagedListViewModel
    {
        public IPagedList<ClaimsViewModel> listClaims { get; set; }
        public List<RoleListViewModel> listRoles { get; set; } = new List<RoleListViewModel>();
        public string roleId { get; set; }
    }
}
