namespace CoreApi.Models
{
    public class UserInfo : IdentityUser<long>
    {
        public int AccessCode { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = "";
        [StringLength(50)]
        public string MiddleName { get; set; } = "";
        [StringLength(50)]
        public string LastName { get; set; } = "";
        [Url]
        [StringLength(256)]
        public string ProfileImageUrl { get; set; } = "https://placehold.co/200?text=Placeholder";
        [StringLength(1)]
        public string Gender { get; set; } = "O";
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
    }
}