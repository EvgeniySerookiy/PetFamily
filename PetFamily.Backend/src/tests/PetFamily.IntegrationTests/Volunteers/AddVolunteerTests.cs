using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

namespace PetFamily.IntegrationTests.Volunteers;

public class AddVolunteerTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, AddVolunteerCommand> _sut;

    public AddVolunteerTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddVolunteerCommand>>();
    }

    [Fact]
    public async Task Create_Volunteer_To_Database_Succeeds()
    {
        // Arrange
        var command = CreateAddVolunteerCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);

        var addedVolunteer = await ReadDbContext.Volunteers.FirstOrDefaultAsync();

        addedVolunteer.Should().NotBeNull();
        addedVolunteer.Id.Should().Be(result.Value);
    }

    private AddVolunteerCommand CreateAddVolunteerCommand()
    {
        return new AddVolunteerCommand(
            CreateMainInfoDto(),
            CreateUpdateSocialNetworksCommand(),
            CreateUpdateRequisitesForHelpCommand());
    }

    private MainInfoDto CreateMainInfoDto()
    {
        return new MainInfoDto(
            CreateFullNameDto(),
            "egor@gmail.com",
            "Описание",
            1,
            "+375446785645");
    }

    private FullNameDto CreateFullNameDto()
    {
        return new FullNameDto(
            "Егор",
            "Казанович",
            "Сергеевич");
    }

    private UpdateSocialNetworksCommand CreateUpdateSocialNetworksCommand()
    {
        var socialNetworkList = new List<SocialNetworkDto>
        {
            new("Telegram", "@Setter")
        };

        return new UpdateSocialNetworksCommand(
            Guid.NewGuid(),
            socialNetworkList);
    }

    private UpdateRequisitesForHelpCommand CreateUpdateRequisitesForHelpCommand()
    {
        var requisitesForHelpList = new List<RequisitesForHelpDto>
        {
            new("Setter", "567890567890")
        };

        return new UpdateRequisitesForHelpCommand(
            Guid.NewGuid(),
            requisitesForHelpList);
    }
}