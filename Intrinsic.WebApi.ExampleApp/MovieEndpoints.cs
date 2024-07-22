using Intrinsic.WebApi.ExampleApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace Intrinsic.WebApi.ExampleApp;

public static class MovieEndpoints
{
    public static void MapMoviesEndpoints(this WebApplication app)
    {
        app.MapPost("/create-movie", async (ExampleWebApiContext dbContext, CancellationToken cancellationToken) =>
        {
            var newMovie = new Movie()
            {
                Id = Guid.NewGuid(),
                Title = $"Movie create at {DateTimeOffset.UtcNow:yyyy-MM-dddd hh:mm:ss.fff}",
                ReleaseYear = 1900 + Random.Shared.Next(0, 100),
            };

            dbContext.Movies.Add(newMovie);
            await dbContext.SaveChangesAsync(cancellationToken);
        });

        app.MapPut("/update-ranpom-movie", async (ExampleWebApiContext dbContext, CancellationToken cancellationToken) =>
        {
            var movie = await dbContext.Movies.FirstAsync(cancellationToken);

            var newMovie = new Movie()
            {
                Id = movie.Id,
                Title = $"Movie created at {DateTimeOffset.UtcNow:yyyy-MM-dddd hh:mm:ss.fff}",
                ReleaseYear = 1900 + Random.Shared.Next(0, 100),
            };

            dbContext.Entry(newMovie).State = EntityState.Modified;

            await dbContext.SaveChangesAsync(cancellationToken);
        });
    }
}
