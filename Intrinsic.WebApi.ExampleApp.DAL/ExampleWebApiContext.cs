using Microsoft.EntityFrameworkCore;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public class ExampleWebApiContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<UserPermission> UserPermissions { get; set; }

    // Needed for EF to apply and create migrations.
    public ExampleWebApiContext() { }

    public ExampleWebApiContext(DbContextOptions<ExampleWebApiContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer();
}
