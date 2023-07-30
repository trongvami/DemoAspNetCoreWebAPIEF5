namespace ServerSide.Models.ResponseModels
{
    public class RoleListResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
