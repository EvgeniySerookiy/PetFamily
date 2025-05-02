using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeletePetPhotos;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;

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
        await WriteDbContext.SaveChangesAsync();


        var socialNetworkList = new List<SocialNetwork>
        {
            SocialNetwork.Create(
                "socialNetwork.NetworkName",
                "socialNetwork.NetworkAddress").Value
        };
        
        var requisitesForHelpList = new List<RequisitesForHelp>
        {
            RequisitesForHelp.Create(
            "requisitesForHelp.Recipient",
            "requisitesForHelp.PaymentDetails").Value
            
        };
        
        var resultVolunteer = Volunteer.Create(
            Guid.NewGuid(),
            FullName.Create("rtyu", "tryu", "dfgh").Value,
            Email.Create("ertyuiop[").Value,
            Description.Create("ertyuiovghjk").Value,
            YearsOfExperience.Create(10).Value,
            PhoneNumber.Create("1234567890").Value,
            TransferRequisitesForHelpsList.Create(requisitesForHelpList).Value,
            TransferSocialNetworkList.Create(socialNetworkList).Value);
        
        var address = Address.Create("rtyu", "tryu", "dfgh", "fds", "gfdsa");
        var size = Size.Create(10, 120);
        var photoPath = PhotoPath.Create(Guid.NewGuid(), "4-1.webp");
        var petPhoto = PetPhoto.Create(photoPath.Value);
        var listPetPhoto = new List<PetPhoto>{petPhoto.Value, petPhoto.Value};
        var resultPet = Pet.Create(
            PetId.Create(Guid.NewGuid()), 
            resultVolunteer.Value.Id,
            PetName.Create("rtyu").Value,
            speciesToCreate.Value.Id,
            breedToCreate.Value.Id,
            listPetPhoto,
            Title.Create("rtyu").Value,
            Description.Create("rtyu").Value,
            Color.Create("rtyu").Value,
            PetHealthInformation.Create("rtyu").Value,
            address.Value,
            PhoneNumber.Create("rtyu").Value,
            size.Value,
            NeuteredStatus.Create(true).Value,
            RabiesVaccinationStatus.Create(true).Value,
            new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc),
            AssistanceStatus.FoundHome,
            DateTime.UtcNow);



        resultVolunteer.Value.AddPet(resultPet.Value);
        WriteDbContext.Volunteers.Add(resultVolunteer.Value);
        await WriteDbContext.SaveChangesAsync();

        
       
        
        var deletePetPhotos = await WriteDbContext.Pets.FirstOrDefaultAsync();
        
        var photoGuid1 = ExtractGuid(deletePetPhotos.PetPhotos[0].PathToStorage.Path);
        var photoGuid2 = ExtractGuid(deletePetPhotos.PetPhotos[1].PathToStorage.Path);
        
        var command = new DeletePetPhotosCommand(resultVolunteer.Value.Id, resultPet.Value.Id, [photoGuid1, photoGuid2]);
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