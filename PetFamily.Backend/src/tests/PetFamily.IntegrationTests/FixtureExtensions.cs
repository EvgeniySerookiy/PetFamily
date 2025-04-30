using AutoFixture;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;
using PetFamily.Domain;

namespace PetFamily.IntegrationTests;

public static class FixtureExtensions
{
    public static AddBreedCommand CreateAddBreedCommand(
        this Fixture fixture,
        Guid speciesId)
    {
        return fixture.Build<AddBreedCommand>()
            .With(c => c.Id, speciesId)
            .With(c => c.Name, "Name")
            .Create();
    }

    public static AddSpeciesCommand CreateAddSpeciesCommand(
        this Fixture fixture)
    {
        return fixture.Build<AddSpeciesCommand>()
            .With(c => c.SpeciesName, "Species name")
            .Create();
    }

    public static CreateVolunteerCommand CreateVolunteerCommand(
        this Fixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>()
            .With(c => c.MainInfo, fixture.CreateMainInfo)
            .With(c => c.UpdateRequisitesForHelp, fixture.CreateUpdateRequisitesForHelpCommand)
            .With(c => c.UpdateSocialNetwork, fixture.CreateUpdateSocialNetworksCommand)
            .Create();
    }

    public static UpdateRequisitesForHelpCommand CreateUpdateRequisitesForHelpCommand(
        this Fixture fixture)
    {
        var singleRequisitesForHelpDto = fixture.CreateRequisitesForHelpDto();
        var requisitesForHelpList = new List<RequisitesForHelpDto>
        {
            singleRequisitesForHelpDto,
            singleRequisitesForHelpDto
        };

        return fixture.Build<UpdateRequisitesForHelpCommand>()
            .With(c => c.RequisitesForHelps, requisitesForHelpList)
            .Create();
    }

    public static UpdateSocialNetworksCommand CreateUpdateSocialNetworksCommand(
        this Fixture fixture)
    {
        var singleSocialNetworkDto = fixture.CreateSocialNetworkDto();
        var socialNetworksList = new List<SocialNetworkDto>
        {
            singleSocialNetworkDto,
            singleSocialNetworkDto
        };

        return fixture.Build<UpdateSocialNetworksCommand>()
            .With(c => c.SocialNetworks, socialNetworksList)
            .Create();
    }

    public static RequisitesForHelpDto CreateRequisitesForHelpDto(
        this Fixture fixture)
    {
        return fixture.Build<RequisitesForHelpDto>()
            .With(c => c.Recipient, "Recipient")
            .With(c => c.PaymentDetails, "Payment details")
            .Create();
    }

    public static SocialNetworkDto CreateSocialNetworkDto(
        this Fixture fixture)
    {
        return fixture.Build<SocialNetworkDto>()
            .With(c => c.NetworkName, "Network name")
            .With(c => c.NetworkAddress, "Network address")
            .Create();
    }

    public static MainInfoDto CreateMainInfo(
        this Fixture fixture)
    {
        return fixture.Build<MainInfoDto>()
            .With(c => c.FullName, fixture.CreateFullNameDto)
            .With(c => c.Email, "sert@gmail.com")
            .With(c => c.Description, "Description")
            .With(c => c.YearsOfExperience, 10)
            .With(c => c.PhoneNumber, "+34567895678")
            .Create();
    }

    public static FullNameDto CreateFullNameDto(
        this Fixture fixture)
    {
        return fixture.Build<FullNameDto>()
            .With(c => c.FirstName, "John")
            .With(c => c.LastName, "Doe")
            .With(c => c.LastName, "Sin")
            .Create();
    }

    public static MainPetInfoCommand CreateMainPetInfoCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        return fixture.Build<MainPetInfoCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.SpeciesId, speciesId)
            .With(c => c.BreedId, breedId)
            .With(c => c.Name, "Ben")
            .With(c => c.Title, "Title")
            .With(c => c.Description, "Description")
            .With(c => c.Color, "Color")
            .With(c => c.PetHealthInformation, "Pet health information")
            .With(c => c.Address, fixture.CreatePetAddressDto)
            .With(c => c.PhoneNumber, "+34567895678")
            .With(c => c.PetSizeDto, fixture.CreatePetSizeDto)
            .With(c => c.IsNeutered, true)
            .With(c => c.IsVaccinated, true)
            .With(c => c.Status, AssistanceStatus.FoundHome)
            .With(c => c.DateOfCreation, DateTime.UtcNow)
            .With(c => c.DateOfBirth, new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc))
            .Create();
    }

    public static PetAddressDto CreatePetAddressDto(
        this IFixture fixture)
    {
        return fixture.Build<PetAddressDto>()
            .With(c => c.Region, "Region")
            .With(c => c.City, "City")
            .With(c => c.Street, "Street")
            .With(c => c.Building, "Building")
            .With(c => c.Apartment, "Apartment")
            .Create();
    }

    public static PetSizeDto CreatePetSizeDto(
        this IFixture fixture)
    {
        return fixture.Build<PetSizeDto>()
            .With(c => c.Height, 10)
            .With(c => c.Weight, 10)
            .Create();
    }
}