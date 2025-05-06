using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.MovePets;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Pets;

public class MovePetsTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, MovePetsCommand> _sut;

    public MovePetsTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MovePetsCommand>>();
    }

    [Fact]
    public async Task Move_Pets_Succeeds()
    {
        // Arrange
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesWriteRepository.Add(createSpecies);

        var createVolunteer = SharedTestsSeeder.CreateVolunteer();

        var createPet1 = SharedTestsSeeder.CreatePet(
            createVolunteer.Id.Value, 
            createSpecies.Id, 
            createBreed.Id);
        
        var createPet2 = SharedTestsSeeder.CreatePet(
            createVolunteer.Id.Value, 
            createSpecies.Id, 
            createBreed.Id);
        
        var createPet3 = SharedTestsSeeder.CreatePet(
            createVolunteer.Id.Value, 
            createSpecies.Id, 
            createBreed.Id);
        
        createVolunteer.AddPet(createPet1);
        createVolunteer.AddPet(createPet2);
        createVolunteer.AddPet(createPet3);
        await VolunteersWriteRepository.Add(createVolunteer);

        var command = new MovePetsCommand(createVolunteer.Id, 3, 2);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var movePet = await ReadDbContext.Pets
            .FirstOrDefaultAsync(m => m.Id == createPet3.Id);

        movePet.Should().NotBeNull();
        movePet.Position.Should().Be(2);
    }

    [Fact]
    public async Task Move_Pets_With_Volunteer_Not_Found_Fails()
    {
        // Arrange
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
        var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
        var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");

        createSpecies.AddBreed(createBreed);
        await SpeciesWriteRepository.Add(createSpecies);

        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersWriteRepository.Add(createVolunteer);

        var command = new MovePetsCommand(createVolunteer.Id, 3, 2);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.InvalidRequest(command.CurrentPosition));
    }
}