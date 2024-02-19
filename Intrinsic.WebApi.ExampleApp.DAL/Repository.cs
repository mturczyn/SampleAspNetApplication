using Microsoft.EntityFrameworkCore;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public interface IRepository
{
    Task<User[]> GetUsersAsync(CancellationToken cancellationToken);
}

public class Repository : IRepository
{
    private readonly ExampleWebApiContext _context;

    public Repository(ExampleWebApiContext context)
    {
        _context = context;
    }

    public async Task<User[]> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToArrayAsync(cancellationToken);
    }
}
