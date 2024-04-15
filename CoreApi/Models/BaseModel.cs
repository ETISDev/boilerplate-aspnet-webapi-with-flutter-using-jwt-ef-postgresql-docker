namespace CoreApi.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedById { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
        public virtual UserInfo CreatedBy { get; set; } = null!;
        public virtual UserInfo UpdatedBy { get; set; } = null!;
        public virtual UserInfo DeletedBy { get; set; } = null!;
    }

}