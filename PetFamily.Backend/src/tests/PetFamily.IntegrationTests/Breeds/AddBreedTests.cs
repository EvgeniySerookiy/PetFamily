using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests.Breeds;

public class AddBreedTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;

    public AddBreedTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }

    [Fact]
    public async Task Add_Breed_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = SharedTestsSeeder.CreateSpecies("Собака");
        await SpeciesWriteRepository.Add(speciesToCreate);
        
        var command = new AddBreedCommand(speciesToCreate.Id.Value, "Сеттер");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var breedExists = await ReadDbContext.Breeds
            .FirstOrDefaultAsync(s => s.Id == result.Value);
        
        breedExists.Should().NotBeNull();
        breedExists.Id.Should().Be(result.Value);
    }
    
    [Fact]
    public async Task Add_Breed_To_Database_When_Species_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = SharedTestsSeeder.CreateSpecies("Собака");
        await SpeciesWriteRepository.Add(speciesToCreate);

        var speciesId = SpeciesId.NewSpeciesId().Value;
        
        var command = new AddBreedCommand(speciesId, "Сеттер");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        
        var expectedError = Errors.Species.NotFound(speciesId);

        result.Error.Should().Contain(expectedError);
    }
    
    [Fact]
    public async Task Add_Breed_To_Database_When_Breed_Name_Already_Exists_Fails()
    {
        // Arrange
        var speciesToCreate = SharedTestsSeeder.CreateSpecies("Собака");
        var breedToCreate = SharedTestsSeeder.CreateBreed("Сеттер");

        speciesToCreate.AddBreed(breedToCreate);
        await SpeciesWriteRepository.Add(speciesToCreate);
        
        var command = new AddBreedCommand(speciesToCreate.Id.Value, "Сеттер");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.Breed.AlreadyExist());
    }
}