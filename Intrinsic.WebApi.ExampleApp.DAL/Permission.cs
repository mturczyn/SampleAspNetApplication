namespace Intrinsic.WebApi.ExampleApp.DAL;

public class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<UserPermission> UserPermissions { get; set; }
}
