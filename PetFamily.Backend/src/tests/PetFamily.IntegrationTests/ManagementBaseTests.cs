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
    protected readonly ISpeciesWriteRepository SpeciesWriteRepository;
    protected readonly IVolunteersWriteRepository VolunteersWriteRepository;

    protected ManagementBaseTests(
        IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        //ReadDbContext = Scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        SpeciesWriteRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesWriteRepository>();
        VolunteersWriteRepository = Scope.ServiceProvider.GetRequiredService<IVolunteersWriteRepository>();
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