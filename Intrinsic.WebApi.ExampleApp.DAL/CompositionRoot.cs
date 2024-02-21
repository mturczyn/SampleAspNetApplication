using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intrinsic.WebApi.ExampleApp.DAL;

public static class CompositionRoot
{
    public static IServiceCollection AddApplicationDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        return services
            .AddSqlServer<ExampleWebApiContext>(configuration.GetConnectionString("ExampleWebApiContext"))
            .AddTransient<IRepository, Repository>()
            ;
    }
}
