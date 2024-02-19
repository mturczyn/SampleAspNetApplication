using Intrinsic.WebApi.ExampleApp.AuthManagement;
using Intrinsic.WebApi.ExampleApp.DAL;
using Microsoft.AspNetCore.Authorization;

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
            .AddSqlServer<ExampleWebApiContext>(configuration.GetConnectionString("ExampleWebApiContext"))
            .AddSingleton<IAuthorizationPolicyProvider, AuthPolicyProvider>()
            .AddSingleton<IAuthorizationHandler, PermissionsAuthHandler>()
            .AddTransient<IRepository, Repository>();
    }
}
