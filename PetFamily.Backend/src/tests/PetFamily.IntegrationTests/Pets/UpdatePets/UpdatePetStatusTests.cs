using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePetStatus;
using PetFamily.Domain;

namespace PetFamily.IntegrationTests.Pets.UpdatePets;

public class UpdatePetStatusTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdatePetStatusCommand> _sut;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createAddPetCommandHandler;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;

    public UpdatePetStatusTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdatePetStatusCommand>>();

        _createAddPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();

        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Update_Pet_Status_To_Database_Succeeds()
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

        var command = new UpdatePetStatusCommand(
            resultVolunteer.Value,
            resultPet.Value,
            AssistanceStatus.LookingForHome);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);

        var updateStatusPet = await WriteDbContext.Pets.FirstOrDefaultAsync();
        
        updateStatusPet.Should().NotBeNull();
        (updateStatusPet.Status == AssistanceStatus.LookingForHome).Should().BeTrue();
        updateStatusPet.Id.Value.Should().Be(result.Value);
    }
}