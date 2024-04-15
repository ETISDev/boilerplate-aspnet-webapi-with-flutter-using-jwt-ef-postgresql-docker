
namespace CoreApi.Data
{
    public class CoreContext : IdentityDbContext<UserInfo, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
{
    public CoreContext(DbContextOptions<CoreContext> opt) : base(opt) { }


    #region "User and Role"
    public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region "Configurations"
        // modelBuilder.HasDbFunction(DateTruncMethod).HasName("date_trunc");
        #endregion


        #region "UserRole"      
        modelBuilder.Entity<UserRole>(UserRole =>
        {
            UserRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            UserRole.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

            UserRole.HasOne(ur => ur.User)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
        });
        #endregion

    }
}
}