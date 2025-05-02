using PetFamily.Domain;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.IntegrationTests;

public static class SharedTestsSeeder
{
    public static Domain.SpeciesManagement.Entities.Species CreateSpecies(string name)
    {
        var speciesId = SpeciesId.NewSpeciesId();
        var speciesName = SpeciesName.Create(name);

        return Domain.SpeciesManagement.Entities.Species.Create(
            speciesId,
            speciesName.Value,
            []).Value;
    }
    
    public static Breed CreateBreed(string name)
    {
        var breedId = BreedId.NewBreedId();
        var breedName = BreedName.Create(name);

        return Breed.Create(
            breedId,
            breedName.Value).Value;
    }

    public static Volunteer CreateVolunteer()
    {
        var socialNetworkList = new List<SocialNetwork>
        {
            SocialNetwork.Create(
                "Telegram",
                "@Setter").Value
        };
        var requisitesForHelpList = new List<RequisitesForHelp>
        {
            RequisitesForHelp.Create(
                "Setter",
                "567890567890").Value
    
        };
        return Volunteer.Create(
            Guid.NewGuid(),
            FullName.Create("Егор", "Казанович", "Сергеевич").Value,
            Email.Create("egor@gmail.com").Value,
            Description.Create("Описание").Value,
            YearsOfExperience.Create(1).Value,
            PhoneNumber.Create("+375446785645").Value,
            TransferRequisitesForHelpsList.Create(requisitesForHelpList).Value,
            TransferSocialNetworkList.Create(socialNetworkList).Value).Value;
    }

    public static Pet CreatePet(
        VolunteerId volunteerId, 
        SpeciesId speciesId, 
        BreedId breedId)
    {
        return Pet.Create(
            PetId.NewPetId(),
            volunteerId.Value,
            PetName.Create("Бен").Value,
            speciesId,
            breedId,
            CreatePetPhotos(),
            Title.Create("Заглавие").Value,
            Domain.PetManagement.SharedVO.Description.Create("Описание").Value,
            Color.Create("Коричневый").Value,
            Domain.PetManagement.PetVO.PetHealthInformation.Create("Информация").Value,
            Address.Create("Минский", "Солигорск", "Козлова", "11", "72").Value,
            PhoneNumber.Create("+375297867898").Value,
            Size.Create(30, 70).Value,
            Domain.PetManagement.PetVO.NeuteredStatus.Create(false).Value,
            RabiesVaccinationStatus.Create(true).Value,
            new DateTime(2017, 10, 21, 0,0,0, DateTimeKind.Utc),
            AssistanceStatus.LookingForHome,
            System.DateTime.UtcNow).Value;
    }

    private static List<PetPhoto> CreatePetPhotos()
    {
        var photoPath = PhotoPath.Create(
            Guid.NewGuid(), 
            "4-1.webp");
        
        var petPhoto = PetPhoto.Create(photoPath.Value);
        
        return [petPhoto.Value, petPhoto.Value];
    }
}