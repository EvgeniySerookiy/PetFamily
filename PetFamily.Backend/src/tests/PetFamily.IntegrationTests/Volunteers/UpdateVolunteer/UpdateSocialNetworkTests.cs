using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Volunteers.UpdateVolunteer;

public class UpdateSocialNetworkTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdateSocialNetworksCommand> _sut;
    
    public UpdateSocialNetworkTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();
    }
    
    [Fact]
    public async Task Update_Social_Networks_To_Database_Succeeds()
    {
        // Arrange
        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersWriteRepository.Add(createVolunteer);

        var listOfSocialNetworkDtos = CreateSocialNetworkDtos();
        
        var command = new UpdateSocialNetworksCommand(createVolunteer.Id, listOfSocialNetworkDtos);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);
        
        var updateVolunteer = await WriteDbContext.Volunteers.FirstOrDefaultAsync();
        
        updateVolunteer.Should().NotBeNull();
        updateVolunteer.TransferSocialNetworkList.SocialNetworks[0].NetworkAddress
            .Should().Be(listOfSocialNetworkDtos[0].NetworkAddress);
        updateVolunteer.Id.Value.Should().Be(result.Value);
    }
    
    [Fact]
    public async Task Update_Social_Networks_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var command = new UpdateSocialNetworksCommand(volunteerId, CreateSocialNetworkDtos());
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }
    
    private List<SocialNetworkDto> CreateSocialNetworkDtos()
    {
        var singleSocialNetworkDto = CreateSocialNetworkDto();
        return
        [
            singleSocialNetworkDto,
            singleSocialNetworkDto
        ];
    }

    private SocialNetworkDto CreateSocialNetworkDto()
    {
        return new SocialNetworkDto(
            "facebook",
            "@labrador");
    }
}