using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteBreed;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests.Breeds;

public class DeleteBreedTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeleteBreedCommand> _sut;
    
    public DeleteBreedTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
    }

    [Fact]
    public async Task Delete_Breed_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);
        
        var command = new DeleteBreedCommand(
            speciesToCreate.Value.Id.Value, 
            breedToCreate.Value.Id.Value);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        ReadDbContext.Breeds.Count().Should().Be(0);
    }
    
    [Fact]
    public async Task Delete_Breed_To_Database_When_Breed_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);
        
        var breedId = BreedId.NewBreedId().Value;
        
        var command = new DeleteBreedCommand(
            speciesToCreate.Value.Id.Value, 
            breedId);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.Breed.NotFound(breedId));
    }
}