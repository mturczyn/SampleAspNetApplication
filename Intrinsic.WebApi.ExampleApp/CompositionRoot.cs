using Intrinsic.WebApi.ExampleApp.AuthManagement;
using Intrinsic.WebApi.ExampleApp.DAL;
using Intrinsic.WebApi.ExampleApp.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Intrinsic.WebApi.ExampleApp;

public static class CompositionRoot
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication()
            .AddCookie();

        return services
            .AddApplicationDal(configuration)
            .AddSingleton<IAuthorizationPolicyProvider, AuthPolicyProvider>()
            .AddSingleton<IAuthorizationHandler, PermissionsAuthHandler>()
            .AddProblemDetails()
            .AddSingleton<ExceptionToProblemDetailsHandler>()
            .AddExceptionHandler<ExceptionToProblemDetailsHandler>((opts, handler) => { })
            ;
    }
}
