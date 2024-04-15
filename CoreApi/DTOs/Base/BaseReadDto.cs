namespace CoreApi.DTOs.Base
{
    public class BaseReadDto
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long CreatedById { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public long UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }
    }

}