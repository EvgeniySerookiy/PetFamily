using AutoFixture;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests;

public class ManagementBaseTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly ISpeciesRepository SpeciesRepository;
    protected readonly IFileProvider FileProvider;

    protected ManagementBaseTests(
        IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        SpeciesRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        FileProvider = Scope.ServiceProvider.GetRequiredService<IFileProvider>();
    }
    
    protected Result<Domain.SpeciesManagement.Entities.Species> CreateSpecies(string name)
    {
        var speciesId = SpeciesId.NewSpeciesId();
        var speciesName = SpeciesName.Create(name);

        return Domain.SpeciesManagement.Entities.Species.Create(
            speciesId,
            speciesName.Value,
            []);
    }
    
    protected Result<Breed, Error> CreateBreed(string name)
    {
        var breedId = BreedId.NewBreedId();
        var breedName = BreedName.Create(name);

        return Breed.Create(
            breedId,
            breedName.Value);
    }
    
    protected AddPetPhotosCommand CreateAddPetPhotosCommand(
        Guid volunteerId,
        Guid petId)
    {
        return new AddPetPhotosCommand(
            volunteerId,
            petId,
            [
                CreatePhotoDto(),
                CreatePhotoDto()
            ]);
    }

    protected CreatePhotoDto CreatePhotoDto()
    {
        return new CreatePhotoDto(
            Stream.Null, 
            "4-1.webp",
            "4-1.webp");
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