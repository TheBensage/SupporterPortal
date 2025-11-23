using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupporterPortal.Application.Services;
using SupporterPortal.Infrastructure.Services;

namespace SupporterPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICrmService, CrmService>();
        return services;
    }
}
