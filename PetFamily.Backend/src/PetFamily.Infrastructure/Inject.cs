using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Application.Photos;
using PetFamily.Application.Providers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddDbContextFactory<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<SoftDeleteCleanupService>();
        services.AddHostedService<PhotosCleanupBackgroundService>();
        services.AddSingleton<IMessageQueue<IEnumerable<PhotoInfo>>, InMemoryMessageQueue<IEnumerable<PhotoInfo>>>();
        services.AddMinio(configuration);
        
        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>() 
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}