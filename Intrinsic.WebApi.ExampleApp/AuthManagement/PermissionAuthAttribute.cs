using Intrinsic.WebApi.ExampleApp.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace Intrinsic.WebApi.ExampleApp.AuthManagement;

public class PermissionAuthAttribute : AuthorizeAttribute
{
    private const string SEPARATOR = "~!_!~";

    public PermissionAuthAttribute(
        PermissionOperator @operator,
        params Permission[] permissions)
    {
        Policy = GeneratePolicyName(
            @operator,
            permissions);
    }

    public static (PermissionOperator Operator, Permission[] Permissions) 
        GetRequiredPermissions(string policyName)
    {
        var items = policyName.Split(SEPARATOR);
        
        if(items.Length < 2)
        {
            throw new ArgumentException("invalid policy name");
        }

        var op = items[0];
        var requiredPerms = items[1..];

        return (
            Operator: Enum.Parse<PermissionOperator>(op),
            Permissions: requiredPerms
                .Select(Enum.Parse<Permission>)
                .ToArray()
            );
    }

    public static string GeneratePolicyName(
        PermissionOperator @operator,
        params Permission[] permissions)
    {
        return string.Join(
            SEPARATOR,
            new[] { $"{@operator}", }.Union(
                permissions
                    .Select(x => $"{x}")));
    }
}
