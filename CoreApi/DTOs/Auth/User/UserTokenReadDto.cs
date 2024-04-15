namespace CoreApi.DTOs.User
{
    public class UserTokenReadDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? ProfileImageUrl { get; set; }
    }
}