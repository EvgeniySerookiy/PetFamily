using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;
using PetFamily.Domain;

namespace PetFamily.IntegrationTests.Pets.UpdatePets;

public class UpdatePetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdatePetCommand> _sut;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createAddPetCommandHandler;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;

    public UpdatePetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();

        _createAddPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();

        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Update_Pet_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var createPetCommand = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);
        var resultPet = await _createAddPetCommandHandler.Handle(createPetCommand);
        
        var command = CreateUpdatePetCommand(
            resultVolunteer.Value, 
            resultPet.Value, 
            speciesToCreate.Value.Id.Value, 
            breedToCreate.Value.Id.Value);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);

        var updatePet = await ReadDbContext.Pets.FirstOrDefaultAsync();
        
        updatePet.Should().NotBeNull();
        (updatePet.PetName == command.Name).Should().BeTrue();
        updatePet.Id.Should().Be(result.Value);
    }

    private UpdatePetCommand CreateUpdatePetCommand(
        Guid volunteerId,
        Guid petId,
        Guid speciesId,
        Guid breedId)
    {
        return new UpdatePetCommand(
            volunteerId,
            petId,
            speciesId,
            breedId,
            "NewBen",
            "NewTitle",
            "NewDescription",
            "NewColor",
            "NewPet health information",
            Fixture.CreatePetAddressDto(),
            "+3456789045678",
            Fixture.CreatePetSizeDto(),
            false,
            false,
            System.DateTime.UtcNow, 
            AssistanceStatus.FoundHome,
            new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc));
    }
}