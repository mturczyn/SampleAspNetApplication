namespace Intrinsic.WebApi.ExampleApp.DAL;

public class User
{
    public int Id { get; set; }

    public int ExternalId { get; set; }

    public string Email { get; set; }

    public ICollection<UserPermission> UserPermissions { get; set; }
}
