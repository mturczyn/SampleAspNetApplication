using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public class ExampleWebApiContext : IdentityDbContext
{
    // Needed for EF to apply and create migrations.
    public ExampleWebApiContext() { }

    public ExampleWebApiContext(DbContextOptions<ExampleWebApiContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer();

    public DbSet<Movie> Movies { get; set; }
}
