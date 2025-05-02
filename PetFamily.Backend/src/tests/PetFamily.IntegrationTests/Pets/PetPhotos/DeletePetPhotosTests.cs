using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeletePetPhotos;

namespace PetFamily.IntegrationTests.Pets.PetPhotos;

public class DeletePetPhotosTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeletePetPhotosCommand> _sut;
        
    public DeletePetPhotosTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeletePetPhotosCommand>>();
    }
    
    [Fact]
    public async Task Delete_Pet_Photos_To_Database_Succeeds()
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
        
        var photoGuid1 = ExtractGuid(createPet.PetPhotos[0].PathToStorage.Path);
        var photoGuid2 = ExtractGuid(createPet.PetPhotos[1].PathToStorage.Path);
        
        var command = new DeletePetPhotosCommand(
            createVolunteer.Id, 
            createPet.Id, 
            [photoGuid1, photoGuid2]);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var updatedPet = await ReadDbContext.Pets
            .FirstOrDefaultAsync();

        updatedPet.Should().NotBeNull();
        updatedPet.PetPhotos.Should().HaveCount(0);
    }
    
    private Guid ExtractGuid(string input)
    {
        var parts = input.Split('.');
        return Guid.Parse(parts[0]);
    }
}