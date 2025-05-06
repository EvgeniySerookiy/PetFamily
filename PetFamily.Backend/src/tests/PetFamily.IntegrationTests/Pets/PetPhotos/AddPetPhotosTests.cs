// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using PetFamily.Application.Abstractions;
// using PetFamily.Application.Dtos.PetDTOs;
// using PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;
// using PetFamily.Domain.Shared.ErrorContext;
//
// namespace PetFamily.IntegrationTests.Pets.PetPhotos;
//
// public class AddPetPhotosTests : ManagementBaseTests
// {
//     private readonly ICommandHandler<Guid, AddPetPhotosCommand> _sut;
//         
//     public AddPetPhotosTests(IntegrationTestsWebFactory factory) : base(factory)
//     {
//         _sut = Scope.ServiceProvider
//             .GetRequiredService<ICommandHandler<Guid, AddPetPhotosCommand>>();
//     }
//
//     [Fact]
//     public async Task Add_Pet_Photos_To_Database_Succeeds()
//     {
//         // Arrange
//         var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
//         var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");
//
//         createSpecies.AddBreed(createBreed);
//         await SpeciesRepository.Add(createSpecies);
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
//         var command = CreateAddPetPhotosCommand(createVolunteer.Id, createPet.Id);
//
//         // Act
//         Factory.SetupSuccessPhotoProviderSubstitute();
//         var result = await _sut.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.IsSuccess.Should().BeTrue();
//         result.Value.Should().NotBeEmpty();
//
//         var updatedPet = await WriteDbContext.Pets
//             .FirstOrDefaultAsync();
//         
//         updatedPet.Should().NotBeNull();
//         updatedPet.PetPhotos.Should().HaveCount(4);
//     }
//
//     [Fact]
//     public async Task Add_Pet_Photos_To_Database_When_Upload_Files_Not_Found_Fails()
//     {
//         // Arrange
//         var createSpecies = SharedTestsSeeder.CreateSpecies("Собака");
//         var createBreed = SharedTestsSeeder.CreateBreed("Сеттер");
//
//         createSpecies.AddBreed(createBreed);
//         await SpeciesRepository.Add(createSpecies);
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
//         var command = CreateAddPetPhotosCommand(createVolunteer.Id, createPet.Id);
//
//         // Act
//         Factory.SetupFailurePhotoProviderSubstitute();
//         var result = await _sut.Handle(command, CancellationToken.None);
//
//         // Assert
//         result.IsFailure.Should().BeTrue();
//         result.Error.Should().Contain(Errors.General.NotFound());
//     }
//     
//     private AddPetPhotosCommand CreateAddPetPhotosCommand(
//         Guid volunteerId,
//         Guid petId)
//     {
//         return new AddPetPhotosCommand(
//             volunteerId,
//             petId,
//             [
//                 CreatePhotoDto(),
//                 CreatePhotoDto()
//             ]);
//     }
//
//     private CreatePhotoDto CreatePhotoDto()
//     {
//         return new CreatePhotoDto(
//             Stream.Null, 
//             "4-1.webp",
//             "4-1.webp");
//     }
// }