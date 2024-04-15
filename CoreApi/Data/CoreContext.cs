using CoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Data
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
    }
}