
namespace CoreApi.Models
{

    public class UserRole : IdentityUserRole<long>
    {
        public virtual UserInfo User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}