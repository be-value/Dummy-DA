using Entities.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace Entities.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSeedingServices(this IServiceCollection services)
    {
        services.AddSingleton<ISeeder, Seeder>();
        return services;
    }
}