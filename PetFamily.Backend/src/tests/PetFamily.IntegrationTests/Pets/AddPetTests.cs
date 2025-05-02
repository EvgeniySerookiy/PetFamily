using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests.Pets;

public class AddPetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _sut;

    public AddPetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();
    }

    [Fact]
    public async Task Add_Pet_To_Database_Succeeds()
    {
        // Arrange
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesRepository.Add(createSpecies);

        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersRepository.Add(createVolunteer);

        var command = CreateMainPetInfoCommand(
            createVolunteer.Id,
            createSpecies.Id.Value,
            createBreed.Id.Value,
            "Бен");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var addedPet = await ReadDbContext.Pets.FirstOrDefaultAsync();

        addedPet.Should().NotBeNull();
        addedPet.VolunteerId.Should().Be(createVolunteer.Id);
    }

    [Fact]
    public async Task Add_Pet_To_Database_When_Breed_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var speciesId = SpeciesId.NewSpeciesId().Value;
        var breedId = BreedId.NewBreedId().Value;

        var command = CreateMainPetInfoCommand(
            volunteerId, 
            speciesId, 
            breedId,
            "Бен");

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
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesRepository.Add(createSpecies);

        var command = CreateMainPetInfoCommand(
            volunteerId,
            speciesIdTest,
            createBreed.Id.Value,
            "Бен");

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
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesRepository.Add(createSpecies);

        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersRepository.Add(createVolunteer);

        var command = CreateMainPetInfoCommand(
            createVolunteer.Id,
            createSpecies.Id.Value,
            createBreed.Id.Value,
            "");

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
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesRepository.Add(createSpecies);

        var volunteerId = VolunteerId.NewVolunteerId().Value;

        var command = CreateMainPetInfoCommand(
            volunteerId,
            createSpecies.Id.Value,
            createBreed.Id.Value,
            "Бен");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }

    private MainPetInfoCommand CreateMainPetInfoCommand(
        Guid volunteerId, 
        Guid speciesId, 
        Guid breedId, string name)
    {
        return new MainPetInfoCommand(
            volunteerId,
            speciesId,
            breedId,
            name,
            "Заглавие",
            "Описание",
            "Коричневый",
            "Информация",
            CreatePetAddressDto(),
            "+375297867898",
            CreatePetSizeDto(),
            false,
            true,
            new DateTime(2017, 10, 21, 0,0,0, DateTimeKind.Utc),
            AssistanceStatus.LookingForHome,
            DateTime.UtcNow);

    }

    private PetAddressDto CreatePetAddressDto()
    {
        return new PetAddressDto(
            "Минский",
            "Солигорск",
            "Козлова",
            "11",
            "72");
    }

    private PetSizeDto CreatePetSizeDto()
    {
        return new PetSizeDto(
            30,
            70);
    }
}