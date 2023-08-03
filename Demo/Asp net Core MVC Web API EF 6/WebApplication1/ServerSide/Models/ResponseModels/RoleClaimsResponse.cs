namespace ServerSide.Models.ResponseModels
{
    public class RoleClaimsResponse
    {
        public List<ClaimsResponse> listClaims { get; set; } = new List<ClaimsResponse>();
        public List<RoleListResponse> listRoles { get; set; } = new List<RoleListResponse>();
        public string roleId { get; set; }
    }
}
