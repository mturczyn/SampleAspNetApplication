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

        app.MapGet("/get-movies", async (ExampleWebApiContext dbContext, CancellationToken cancellationToken) =>
        {
            var movies = await dbContext.Movies.ToArrayAsync(cancellationToken);
            return movies;
        });

        app.MapPut("/update-to-default", async (ExampleWebApiContext context, CancellationToken cancellationToken) =>
        {
            var concreteId = Guid.Parse("ec6ac7fd-9937-4f84-800f-30e9bf4a1e0d");

            var ids = new Guid[] {
                concreteId,
                Guid.Parse("79183d38-66b6-4059-95a9-5a686c1fbb60"),
                Guid.Parse("060ee0a9-545c-4417-a5c7-6ae2c488d49c"),
            };

            var result = await context.Movies
                .Where(x => ids.Contains(x.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Title, p => "New exciting movie with supaerstars!"));
        });
    }
}