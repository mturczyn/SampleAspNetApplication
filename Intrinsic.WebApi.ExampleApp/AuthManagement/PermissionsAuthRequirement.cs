using Intrinsic.WebApi.ExampleApp.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace Intrinsic.WebApi.ExampleApp.AuthManagement;

public class PermissionsAuthRequirement : IAuthorizationRequirement
{
    public static string ClaimType => "intrinsic-permission";

    // 1 - The operator
    public PermissionOperator PermissionOperator { get; }

    // 2 - The list of permissions passed
    public string[] Permissions { get; }

    public PermissionsAuthRequirement(
        PermissionOperator permissionOperator, Permission[] permissions)
    {
        if (permissions.Length == 0)
        {
            throw new ArgumentException("At least one permission is required.", nameof(permissions));
        }

        PermissionOperator = permissionOperator;
        Permissions = permissions.Select(x => $"{x}").ToArray();
    }
}
