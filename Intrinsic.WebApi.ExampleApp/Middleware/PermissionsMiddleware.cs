//namespace Intrinsic.WebApi.ExampleApp.Middleware;

//public class PermissionsMiddleware
//{
//    private readonly RequestDelegate _next;

//    public PermissionsMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(
//        HttpContext context,
//        IUserPermissionService userPermissionService)
//    {
//        // 1 - if the request is not authenticated, nothing to do
//        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
//        {
//            await _next(context);
//            return;
//        }

//        var cancellationToken = context.RequestAborted;

//        // 2. The 'sub' claim is how we find the user in our system
//        var userSub = context.User.FindFirst(StandardJwtClaimTypes.Subject)?.Value;
//        if (string.IsNullOrEmpty(userSub))
//        {
//            await context.WriteAccessDeniedResponse(
//              "User 'sub' claim is required",
//              cancellationToken: cancellationToken);
//            return;
//        }

//        // 3 - Now we try to get the user permissions (as ClaimsIdentity)
//        var permissionsIdentity = await permissionService
//            .GetUserPermissionsIdentity(userSub, cancellationToken);
//        if (permissionsIdentity == null)
//        {
//            await context.WriteAccessDeniedResponse(cancellationToken: cancellationToken);
//            return;
//        }

//        // 4 - User has permissions
//        // so we add the extra identity to the ClaimsPrincipal
//        context.User.AddIdentity(permissionsIdentity);
//        await _next(context);
//    }
//}
