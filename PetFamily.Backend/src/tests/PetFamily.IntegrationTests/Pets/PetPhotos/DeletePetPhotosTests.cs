using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeletePetPhotos;

namespace PetFamily.IntegrationTests.Pets.PetPhotos;

public class DeletePetPhotosTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeletePetPhotosCommand> _sut;
    private readonly ICommandHandler<Guid, AddPetPhotosCommand> _createAddPetPhotosCommandHandler;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;
    private readonly ICommandHandler<Guid, MainPetInfoCommand> _createPetCommandHandler;
        
    public DeletePetPhotosTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeletePetPhotosCommand>>();
        
        _createAddPetPhotosCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddPetPhotosCommand>>();
        
        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
        
        _createPetCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, MainPetInfoCommand>>();
    }
    
    [Fact]
    public async Task Delete_Pet_Photos_To_Database_Succeeds()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("String");
        var breedToCreate = CreateBreed("String");

        speciesToCreate.Value.AddBreed(breedToCreate.Value);
        await SpeciesRepository.Add(speciesToCreate.Value);

        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var resultVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);

        var createMainPetInfoCommand = Fixture.CreateMainPetInfoCommand(
            resultVolunteer.Value,
            speciesToCreate.Value.Id.Value,
            breedToCreate.Value.Id.Value);
        
        var resultPet = await _createPetCommandHandler.Handle(createMainPetInfoCommand, CancellationToken.None);
        
        var createAddPetPhotosCommand = CreateAddPetPhotosCommand(resultVolunteer.Value, resultPet.Value);
        
        var resultAddPhotos = await _createAddPetPhotosCommandHandler.Handle(createAddPetPhotosCommand, CancellationToken.None);

        var deletePetPhotos = await WriteDbContext.Pets.FirstOrDefaultAsync();

        var photoGuid1 = ExtractGuid(deletePetPhotos.PetPhotos[0].PathToStorage.Path);
        var photoGuid2 = ExtractGuid(deletePetPhotos.PetPhotos[1].PathToStorage.Path);
        
        var command = new DeletePetPhotosCommand(resultVolunteer.Value, resultPet.Value, [photoGuid1, photoGuid2]);
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        // result.Value.Should().NotBeEmpty();
        //
        // var updatedPet = await WriteDbContext.Pets
        //     .FirstOrDefaultAsync();
        //
        // updatedPet.Should().NotBeNull();
        // updatedPet.PetPhotos.Should().HaveCount(2);
    }
    
    private Guid ExtractGuid(string input)
    {
        var parts = input.Split('.');
        return Guid.Parse(parts[0]);
    }
}