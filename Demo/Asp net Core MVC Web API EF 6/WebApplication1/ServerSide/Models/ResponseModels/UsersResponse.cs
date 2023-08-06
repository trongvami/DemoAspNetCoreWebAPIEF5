namespace ServerSide.Models.ResponseModels
{
    public class UsersResponse
    {
        public string userId { get; set; }
        public string userEmail { get; set; }
        public string userFullName { get; set; }
        public bool IsSelected { get; set; }
    }
}
