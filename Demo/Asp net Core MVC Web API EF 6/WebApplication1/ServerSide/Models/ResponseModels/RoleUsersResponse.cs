namespace ServerSide.Models.ResponseModels
{
    public class RoleUsersResponse
    {
        public string roleId {  get; set; }
        public List<RoleListResponse> listRoles { get; set; } = new List<RoleListResponse>();
        public List<UsersResponse> userList { get; set; } = new List<UsersResponse>();
    }
}
