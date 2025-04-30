using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeletePet;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Pets;

public class DeletePetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeletePetCommand> _sut;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createAddPetCommandHandler;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;

    public DeletePetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeletePetCommand>>();

        _createAddPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();

        _createVolunteerCommandHandler =
            Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Delete_Pet_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var mainPetInfoCommand = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        var addedPet = await _createAddPetCommandHandler.Handle(mainPetInfoCommand);

        var command = new DeletePetCommand(resultVolunteer.Value, addedPet.Value);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deletedPet = await ReadDbContext.Pets
            .FirstOrDefaultAsync(d => d.Id == result.Value);

        deletedPet.Should().NotBeNull();
        deletedPet.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Pet_To_Database_When_Pet_Not_Found_Fails()
    {
        // Arrange
        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var petId = PetId.NewPetId().Value;

        var command = new DeletePetCommand(resultVolunteer.Value, petId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(petId));
    }
}