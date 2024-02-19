using Microsoft.AspNetCore.Authorization;

namespace Intrinsic.WebApi.ExampleApp.AuthManagement;

public class PermissionsAuthHandler : AuthorizationHandler<PermissionsAuthRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionsAuthRequirement requirement)
    {
        if (requirement.PermissionOperator == PermissionOperator.And)
        {
            foreach (var permission in requirement.Permissions)
            {
                if (!context.User.
                    HasClaim(PermissionsAuthRequirement.ClaimType, permission))
                {
                    // If the user lacks ANY of the required permissions
                    // we mark it as failed.
                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            // identity has all required permissions
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        foreach (var permission in requirement.Permissions)
        {
            if (context.User.HasClaim(PermissionsAuthRequirement.ClaimType, permission))
            {
                // In the OR case, as soon as we found a matching permission
                // we can already mark it as Succeed
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        // identity does not have any of the required permissions
        context.Fail();
        return Task.CompletedTask;
    }
}