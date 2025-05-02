using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;
using PetFamily.Domain;

namespace PetFamily.IntegrationTests.Pets.UpdatePets;

public class UpdatePetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdatePetCommand> _sut;

    public UpdatePetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();
    }

    [Fact]
    public async Task Update_Pet_To_Database_Succeeds()
    {
        // Arrange
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesRepository.Add(createSpecies);

        var createVolunteer = SharedTestsSeeder.CreateVolunteer();

        var createPet = SharedTestsSeeder.CreatePet(
            createVolunteer.Id.Value, 
            createSpecies.Id, 
            createBreed.Id);
        
        createVolunteer.AddPet(createPet);
        await VolunteersRepository.Add(createVolunteer);
        
        var command = CreateUpdatePetCommand(
            createVolunteer.Id, 
            createPet.Id, 
            createSpecies.Id.Value, 
            createBreed.Id.Value);
        
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
            "Бонд",
            "Новое заглавие",
            "Новое описание",
            "Черный",
            "Новая информация",
            new PetAddressDto("Минский", "Минск", "Белинского", "9", "39"),
            "+3456789045678",
            new PetSizeDto(40, 80),
            false,
            false,
            System.DateTime.UtcNow, 
            AssistanceStatus.FoundHome,
            new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc));
    }
}