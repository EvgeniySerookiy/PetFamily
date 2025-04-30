using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.MovePets;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Pets;

public class MovePetsTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, MovePetsCommand> _sut;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createAddPetCommandHandler;

    public MovePetsTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MovePetsCommand>>();

        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();

        _createAddPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();
    }

    [Fact]
    public async Task Move_Pets_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var petCommand1 = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        var petCommand2 = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        var petCommand3 = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        var addedPet1 = await _createAddPetCommandHandler.Handle(petCommand1);
        var addedPet2 = await _createAddPetCommandHandler.Handle(petCommand2);
        var addedPet3 = await _createAddPetCommandHandler.Handle(petCommand3);

        var command = new MovePetsCommand(resultVolunteer.Value, 3, 2);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var movePet = await ReadDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == addedPet3.Value);

        movePet.Should().NotBeNull();
        movePet.Position.Should().Be(2);
    }

    [Fact]
    public async Task Move_Pets_With_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var volunteerId = VolunteerId.NewVolunteerId().Value;

        var command = new MovePetsCommand(volunteerId, 3, 2);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }

    [Fact]
    public async Task Move_Pets_With_Pet_Invalid_Request_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var petCommand1 = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        var addedPet1 = await _createAddPetCommandHandler.Handle(petCommand1);

        var command = new MovePetsCommand(resultVolunteer.Value, 3, 2);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.InvalidRequest(command.CurrentPosition));
    }
}