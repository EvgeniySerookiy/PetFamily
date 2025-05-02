using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteSpecies;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests.Species;

public class DeleteSpeciesTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;

    public DeleteSpeciesTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
    }
    
    [Fact]
    public async Task Delete_Species_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = SharedTestsSeeder.CreateSpecies("Собака");
        
        await SpeciesRepository.Add(speciesToCreate);
        
        var command = new DeleteSpeciesCommand(speciesToCreate.Id.Value);
            
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ReadDbContext.Species.Count().Should().Be(0);
    }

    [Fact]
    public async Task Delete_Species_To_Database_When_Species_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = SharedTestsSeeder.CreateSpecies("Собака");
        
        await SpeciesRepository.Add(speciesToCreate);

        var speciesId = SpeciesId.NewSpeciesId().Value;
        var command = new DeleteSpeciesCommand(speciesId);
            
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.Species.NotFound(speciesId));
    }
}