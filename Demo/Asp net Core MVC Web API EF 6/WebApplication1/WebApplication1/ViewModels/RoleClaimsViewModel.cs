namespace WebApplication1.ViewModels
{
    public class RoleClaimsViewModel
    {
        public List<ClaimsViewModel> listClaims { get; set; } = new List<ClaimsViewModel>();
        public List<RoleListViewModel> listRoles { get; set; } = new List<RoleListViewModel>();
        public string roleId { get; set; }
    }
}
