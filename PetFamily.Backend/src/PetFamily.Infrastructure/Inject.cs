using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContex>();
        services.AddDbContextFactory<ApplicationDbContex>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddHostedService<SoftDeleteCleanupService>();
        return services;
    }
}