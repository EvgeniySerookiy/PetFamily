using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using PetFamily.API;
using PetFamily.Application.Database;
using PetFamily.Application.Photos;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IFileProvider _fileProviderSubstitute = Substitute.For<IFileProvider>();
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));

        var readContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(ReadDbContext));
        
        var fileProvider = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IFileProvider));

        if (writeContext is not null)
            services.Remove(writeContext);

        if (readContext is not null)
            services.Remove(readContext);

        if (fileProvider is not null)
            services.Remove(fileProvider);
        
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));

        services.AddScoped<ReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));

        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));

        services.AddTransient<IFileProvider>(_ => _fileProviderSubstitute);
    }
    
    public void SetupSuccessPhotoProviderSubstitute()
    {
        var response = new List<PhotoPath>().AsReadOnly();
        _fileProviderSubstitute
            .UploadPhotos(Arg.Any<IEnumerable<PhotoData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(response));
    }
    public void SetupFailurePhotoProviderSubstitute()
    {
        _fileProviderSubstitute
            .UploadPhotos(Arg.Any<IEnumerable<PhotoData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<IReadOnlyList<PhotoPath>, Error>(Errors.General.NotFound()));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await writeDbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
            }
        );
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}