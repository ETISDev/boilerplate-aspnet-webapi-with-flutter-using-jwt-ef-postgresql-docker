namespace CoreApi.DTOs.User
{
    public class UserInfoUpdateDto
    {

        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = "";
        [StringLength(50)]
        public string MiddleName { get; set; } = "";
        [StringLength(50)]
        public string LastName { get; set; } = "";
        [Url]
        [StringLength(256)]
        public string? ProfileImageUrl { get; set; }
        [StringLength(1)]
        public string Gender { get; set; } = "O";
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

}