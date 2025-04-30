using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests.Pets;

public class AddPetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _sut;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;

    public AddPetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();

        _createVolunteerCommandHandler =
            Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Add_Pet_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var command = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var addedPet = await ReadDbContext.Pets.FirstOrDefaultAsync();

        addedPet.Should().NotBeNull();
        addedPet.VolunteerId.Should().Be(resultVolunteer.Value);
    }

    [Fact]
    public async Task Add_Pet_To_Database_When_Breed_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var speciesId = SpeciesId.NewSpeciesId().Value;
        var breedId = BreedId.NewBreedId().Value;

        var command = Fixture.CreateMainPetInfoCommand(volunteerId, speciesId, breedId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(breedId));
    }

    [Fact]
    public async Task Add_Pet_To_Database_When_Breed_Species_Mismatch_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var speciesIdTest = SpeciesId.NewSpeciesId().Value;
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var command = Fixture.CreateMainPetInfoCommand(
            volunteerId,
            speciesIdTest,
            breedToCreate.Value.Id.Value);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(speciesIdTest));
    }

    [Fact]
    public async Task Add_Pet_To_Database_With_Empty_Name_Error()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var command = Fixture.Build<MainPetInfoCommand>()
            .With(c => c.VolunteerId, resultVolunteer.Value)
            .With(c => c.SpeciesId, speciesToCreate.Value.Id.Value)
            .With(c => c.BreedId, breedToCreate.Value.Id.Value)
            .With(c => c.Name, "")
            .With(c => c.Title, "Title")
            .With(c => c.Description, "Description")
            .With(c => c.Color, "Color")
            .With(c => c.PetHealthInformation, "Pet health information")
            .With(c => c.Address, Fixture.CreatePetAddressDto)
            .With(c => c.PhoneNumber, "+34567895678")
            .With(c => c.PetSizeDto, Fixture.CreatePetSizeDto)
            .With(c => c.IsNeutered, true)
            .With(c => c.IsVaccinated, true)
            .With(c => c.Status, AssistanceStatus.FoundHome)
            .With(c => c.DateOfCreation, DateTime.UtcNow)
            .With(c => c.DateOfBirth, new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc))
            .Create();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        var expectedError = Error.Validation(
            "value.is.invalid",
            "Pet name is invalid",
            "Name");

        result.Error.Should().Contain(expectedError);
    }

    [Fact]
    public async Task Add_Pet_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var volunteerId = VolunteerId.NewVolunteerId().Value;

        var command = Fixture.CreateMainPetInfoCommand(
            volunteerId,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }
}