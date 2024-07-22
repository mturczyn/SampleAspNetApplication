using System.ComponentModel.DataAnnotations;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public class Movie
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public int ReleaseYear { get; set; }
}
