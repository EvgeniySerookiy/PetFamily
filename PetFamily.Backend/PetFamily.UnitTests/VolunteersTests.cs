using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.Execution;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    private const int PETS_COUNTS = 5;
    private const int NEXT_SERIAL_NUMBER = 3;
    
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

    private Result<Pet, Error> CreatePet()
    {
        var petId = PetId.NewPetId();
        var petName = PetName.Create("Test");
        var speciesId = SpeciesId.EmptySpeciesId();
        var breedId = BreedId.EmptyBreedId();
        var transferFilesList = TransferFilesList.Create(
            new List<PetPhoto>
            {
                PetPhoto.Create(PhotoPath.Create(Guid.NewGuid(), ".pdf").Value).Value,
                PetPhoto.Create(PhotoPath.Create(Guid.NewGuid(), ".pdf").Value).Value
            });
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
            petName.Value,
            speciesId,
            breedId,
            transferFilesList.Value,
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

    private IEnumerable<Result<Pet, Error>> CreatePets()
    {
        return Enumerable.Range(1, PETS_COUNTS).Select(_ => CreatePet());
    }

    [Fact]
    public void Add_Pet_With_Empty_Pets_First_Return_Success_Result()
    {
        var volunteer = CreateVolunteer();
        var pet = CreatePet();

        var result = volunteer.Value.AddPet(pet.Value);

        var addedPetResult = volunteer.Value.GetPetById(pet.Value.Id);

        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(pet.Value.Id);
        addedPetResult.Value.SerialNumber.Should().Be(SerialNumber.First);
    }

    [Fact]
    public void Add_Pet_With_Other_Pets_Return_Success_Result()
    {
        // Arrange   
        var volunteer = CreateVolunteer();
        var pets = CreatePets();
        var petToAdd = CreatePet();

        foreach (var pet in pets)
            volunteer.Value.AddPet(pet.Value);

        // Act
        var result = volunteer.Value.AddPet(petToAdd.Value);

        // Assert
        var addedPetResult = volunteer.Value.GetPetById(petToAdd.Value.Id);

        var serialNumber = SerialNumber.Create(PETS_COUNTS + 1);

        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(petToAdd.Value.Id);
        addedPetResult.Value.SerialNumber.Should().Be(serialNumber.Value);
    }

    [Fact]
    public void Move_Pet_Should_Return_Out_Of_Range_When_Serial_Number_Is_Invalid()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pets = CreatePets();
        var petToAdd = CreatePet();

        foreach (var pet in pets)
            volunteer.Value.AddPet(pet.Value);
        
        var outOfRangeNumbers = new[]
        {
            PETS_COUNTS - PETS_COUNTS,
            PETS_COUNTS + 1
        };

        using var scope = new AssertionScope();

        foreach (var number in outOfRangeNumbers)
        {
            // Act
            var result = volunteer.Value.MovePet(petToAdd.Value, number);
            
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Should().BeEquivalentTo(UnitResult.Failure(Errors.General.OutOfRange(number)));
        }
    }
    
    [Fact]
    public void Move_Pet_Should_Return_Not_Found_When_Pet_Does_Not_Exist()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pets = CreatePets();
        var petNotAVolunteer = CreatePet();
        
        foreach (var pet in pets)
            volunteer.Value.AddPet(pet.Value);
        
        // Act
        var result = volunteer.Value.MovePet(petNotAVolunteer.Value, PETS_COUNTS);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Should().BeEquivalentTo(UnitResult.Failure(Errors.General.NotFound(petNotAVolunteer.Value.Id)));
    }

    [Fact]
    public void Move_Pet_Should_Return_Success_When_Serial_Numbers_Are_Equal()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var petToMove = CreatePet();
        var petToCheck = CreatePet();
        
        volunteer.Value.AddPet(petToMove.Value);
        volunteer.Value.AddPet(petToCheck.Value);
        
        var currentSerialNumber = petToMove.Value.SerialNumber.Value;
        
        // Act
        var result = volunteer.Value.MovePet(petToMove.Value, currentSerialNumber);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        petToMove.Value.SerialNumber.Value.Should().Be(1);
        petToCheck.Value.SerialNumber.Value.Should().Be(2);
    }

    [Fact]
    public void Move_Pet_Should_Update_Serial_Number()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();
        var pet4 = CreatePet();
        var pet5 = CreatePet();
        
        var pets = new List<Result<Pet, Error>> { pet1, pet2, pet3, pet4, pet5 };
        
        foreach (var pet in pets)
            volunteer.Value.AddPet(pet.Value);

        // Act
        var result = volunteer.Value.MovePet(pet1.Value, NEXT_SERIAL_NUMBER);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Value.SerialNumber.Value.Should().Be(3);
        pet2.Value.SerialNumber.Value.Should().Be(1);
        pet3.Value.SerialNumber.Value.Should().Be(2);
        pet4.Value.SerialNumber.Value.Should().Be(4);
        pet5.Value.SerialNumber.Value.Should().Be(5);
    }
}