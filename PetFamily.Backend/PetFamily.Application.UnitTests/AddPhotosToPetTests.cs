using CSharpFunctionalExtensions;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.UnitTests;

public class AddPhotosToPetTests
{
    private Result<Volunteer, Error> CreateVolunteer()
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("Test", "Test", "Test");
        var email = Email.Create("Test@Test.com");
        var description = Description.Create("Test");
        var yearsOfExperience = YearsOfExperience.Create(10);
        var phoneNumber = PhoneNumber.Create("123456789");
        var transferRequisitesForHelpsList = TransferRequisitesForHelpsList.Create(
            new List<RequisitesForHelp>
            {
                RequisitesForHelp.Create("Test", "Test").Value,
                RequisitesForHelp.Create("Test2", "Test2").Value,
            });
        var transferSocialNetworkList = TransferSocialNetworkList.Create(
            new List<SocialNetwork>
            {
                SocialNetwork.Create("Test", "Test").Value,
                SocialNetwork.Create("Test2", "Test2").Value,
            });

        var volunteer = Volunteer.Create(
            volunteerId.Value,
            fullName.Value,
            email.Value,
            description.Value,
            yearsOfExperience.Value,
            phoneNumber.Value,
            transferRequisitesForHelpsList.Value,
            transferSocialNetworkList.Value);

        return volunteer;
    }

    private Result<Pet, Error> CreatePet(Volunteer volunteer)
    {
        var petId = PetId.NewPetId();
        var volunteerId = volunteer.Id;
        var petName = PetName.Create("Test");
        var speciesId = SpeciesId.EmptySpeciesId();
        var breedId = BreedId.EmptyBreedId();
        var petPhotos = new List<PetPhoto>
        {
            PetPhoto.Create(PhotoPath.Create(Guid.NewGuid(), ".pdf").Value).Value,
            PetPhoto.Create(PhotoPath.Create(Guid.NewGuid(), ".pdf").Value).Value
        };
            
        var title = Title.Create("Test");
        var descriptionPet = Description.Create("Test");
        var color = Color.Create("Test");
        var petHealthInformation = PetHealthInformation.Create("Test");
        var petAddress = Address.Create(
            "Test", "Test", "Test", "Test", "Test");
        var ownerPhoneNumber = PhoneNumber.Create("123456789");
        var size = Size.Create(10, 10);
        var isNeutered = NeuteredStatus.Create(false);
        var isVaccinated = RabiesVaccinationStatus.Create(false);
        var dateOfBirth = DateTime.Now;
        var status = AssistanceStatus.LookingForHome;
        var transferRequisitesForHelpsListPet = TransferRequisitesForHelpsList.Create(
            new List<RequisitesForHelp>
            {
                RequisitesForHelp.Create("Test", "Test").Value,
                RequisitesForHelp.Create("Test2", "Test2").Value,
            });
        var dateOfCreation = DateTime.Now;
        var pet = Pet.Create(
            petId,
            volunteerId,
            petName.Value,
            speciesId,
            breedId,
            petPhotos,
            title.Value,
            descriptionPet.Value,
            color.Value,
            petHealthInformation.Value,
            petAddress.Value,
            ownerPhoneNumber.Value,
            size.Value,
            isNeutered.Value,
            isVaccinated.Value,
            dateOfBirth,
            status,
            dateOfCreation);

        return pet;
    }
    
    // Для создания различных интерфейсов нужно установить пакет Moq(15 видео, 1:47 минут)
    [Fact]
    public void Handle_Should_Add_Photos_To_Pet()
    {
        // Arrange
        // var volunteer = CreateVolunteer();
        // var pet = CreatePet();
        // var command = new AddPetPhotosCommand();

        // Act


        // Assert
    }
}