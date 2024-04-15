namespace CoreApi.Models
{
    public class Role : IdentityRole<long>
    {
        public string? Description { get; set; } = null;
        public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
    }
}