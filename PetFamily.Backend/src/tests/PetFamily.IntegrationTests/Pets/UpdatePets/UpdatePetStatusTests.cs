// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using PetFamily.Application.Abstractions;
// using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePetStatus;
// using PetFamily.Domain;
//
// namespace PetFamily.IntegrationTests.Pets.UpdatePets;
//
// public class UpdatePetStatusTests : ManagementBaseTests
// {
//     private readonly ICommandHandler<Guid, UpdatePetStatusCommand> _sut;
//
//     public UpdatePetStatusTests(
//         IntegrationTestsWebFactory factory) : base(factory)
//     {
//         _sut = Scope.ServiceProvider
//             .GetRequiredService<ICommandHandler<Guid, UpdatePetStatusCommand>>();
//     }
//
//     [Fact]
//     public async Task Update_Pet_Status_To_Database_Succeeds()
//     {
//         // Arrange
//         var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
//         var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");
//
//         createSpecies.AddBreed(createBreed);
//         await SpeciesWriteRepository.Add(createSpecies);
//
//         var createVolunteer = SharedTestsSeeder.CreateVolunteer();
//
//         var createPet = SharedTestsSeeder.CreatePet(
//             createVolunteer.Id.Value, 
//             createSpecies.Id, 
//             createBreed.Id);
//         
//         createVolunteer.AddPet(createPet);
//         await VolunteersRepository.Add(createVolunteer);
//
//         var command = new UpdatePetStatusCommand(
//             createVolunteer.Id,
//             createPet.Id,
//             AssistanceStatus.NeedsHelp);
//
//         // Act
//         var result = await _sut.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.Value.Should().Be(result.Value);
//
//         var updateStatusPet = await WriteDbContext.Pets.FirstOrDefaultAsync();
//         
//         updateStatusPet.Should().NotBeNull();
//         (updateStatusPet.Status == AssistanceStatus.NeedsHelp).Should().BeTrue();
//         updateStatusPet.Id.Value.Should().Be(result.Value);
//     }
// }