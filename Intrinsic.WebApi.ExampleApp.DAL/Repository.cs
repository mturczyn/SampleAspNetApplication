using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public interface IRepository
{
    Task<IdentityUser[]> GetTop10UsersAsync(CancellationToken cancellationToken);
}

public class Repository : IRepository
{
    private readonly ExampleWebApiContext _context;

    public Repository(ExampleWebApiContext context)
    {
        _context = context;
    }

    public async Task<IdentityUser[]> GetTop10UsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.Take(10).ToArrayAsync(cancellationToken);
    }
}
