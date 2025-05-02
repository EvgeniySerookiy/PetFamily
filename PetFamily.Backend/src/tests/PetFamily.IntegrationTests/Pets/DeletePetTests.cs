using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeletePet;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Pets;

public class DeletePetTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeletePetCommand> _sut;

    public DeletePetTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeletePetCommand>>();
    }

    [Fact]
    public async Task Delete_Pet_To_Database_Succeeds()
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

        var command = new DeletePetCommand(createVolunteer.Id, createPet.Id);

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
        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersRepository.Add(createVolunteer);

        var petId = PetId.NewPetId().Value;

        var command = new DeletePetCommand(createVolunteer.Id, petId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(petId));
    }
}