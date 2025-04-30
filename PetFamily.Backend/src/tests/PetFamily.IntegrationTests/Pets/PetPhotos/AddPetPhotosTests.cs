using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Pets.PetPhotos;

public class AddPetPhotosTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, AddPetPhotosCommand> _sut;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createPetCommandHandler;
        
    public AddPetPhotosTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddPetPhotosCommand>>();
        
        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
        
        _createPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();
    }

    [Fact]
    public async Task Add_Pet_Photos_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var createMainPetInfoCommand = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);
        
        var resultPet = await _createPetCommandHandler.Handle(createMainPetInfoCommand, CancellationToken.None);
        
        var command = CreateAddPetPhotosCommand(resultVolunteer.Value, resultPet.Value);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var updatedPet = await WriteDbContext.Pets
            .FirstOrDefaultAsync();
        
        updatedPet.Should().NotBeNull();
        updatedPet.PetPhotos.Should().HaveCount(2);
    }

    [Fact]
    public async Task Add_Pet_Photos_To_Database_When_Upload_Files_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler
            .Handle(createVolunteerCommand);

        var createMainPetInfoCommand = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);
        
        var resultPet = await _createPetCommandHandler
            .Handle(createMainPetInfoCommand, CancellationToken.None);
        
        var command = CreateAddPetPhotosCommand(resultVolunteer.Value, resultPet.Value);

        // Act
        Factory.SetupFailurePhotoProviderSubstitute();
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound());
    }
}