using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Intrinsic.WebApi.ExampleApp.AuthManagement;

public class AuthPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthPolicyProvider(
        IOptions<AuthorizationOptions> options) :
        base(options)
    { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(
        string policyName)
    {
        try
        {
            // Will extract the Operator AND/OR enum from the string
            // Will extract the permissions from the string (Create, Update..)
            var requiredPermissions = PermissionAuthAttribute
                .GetRequiredPermissions(policyName);

            // Here we create the instance of our requirement
            var requirement = new PermissionsAuthRequirement(
                requiredPermissions.Operator,
                requiredPermissions.Permissions);

            // Now we use the builder to create a policy, adding our requirement
            return new AuthorizationPolicyBuilder()
                .AddRequirements(requirement).Build();
        }
        catch (Exception ex)
        {
            return await base.GetPolicyAsync(policyName);
        }
    }
}
