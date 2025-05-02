using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Application.Providers;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests;

public class ManagementBaseTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly ISpeciesRepository SpeciesRepository;
    protected readonly IVolunteersRepository VolunteersRepository;

    protected ManagementBaseTests(
        IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        SpeciesRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        VolunteersRepository = Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }
}