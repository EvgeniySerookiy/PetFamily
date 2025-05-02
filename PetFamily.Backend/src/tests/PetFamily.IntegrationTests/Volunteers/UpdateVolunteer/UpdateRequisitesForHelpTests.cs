using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Volunteers.UpdateVolunteer;

public class UpdateRequisitesForHelpTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdateRequisitesForHelpCommand> _sut;

    public UpdateRequisitesForHelpTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateRequisitesForHelpCommand>>();
    }
    
    [Fact]
    public async Task Update_Requisites_For_Help_To_Database_Succeeds()
    {
        // Arrange
        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersRepository.Add(createVolunteer);

        var listOfRequisitesForHelpDtos = CreateListOfRequisitesForHelpDtos();
        
        var command = new UpdateRequisitesForHelpCommand(createVolunteer.Id, listOfRequisitesForHelpDtos);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);
        
        var updateVolunteer = await WriteDbContext.Volunteers.FirstOrDefaultAsync();
        
        updateVolunteer.Should().NotBeNull();
        updateVolunteer.TransferRequisitesForHelpsList.RequisitesForHelps[0].Recipient.Should().Be(listOfRequisitesForHelpDtos[0].Recipient);
        updateVolunteer.Id.Value.Should().Be(result.Value);
    }
    
    [Fact]
    public async Task Update_Requisites_For_Help_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var command = new UpdateRequisitesForHelpCommand(volunteerId, CreateListOfRequisitesForHelpDtos());
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }

    private List<RequisitesForHelpDto> CreateListOfRequisitesForHelpDtos()
    {
        var singleRequisitesForHelpDto = CreateRequisitesForHelpDto();
        return
        [
            singleRequisitesForHelpDto,
            singleRequisitesForHelpDto
        ];
    }

    private RequisitesForHelpDto CreateRequisitesForHelpDto()
    {
        return new RequisitesForHelpDto(
            "Labrador",
            "67890678956789");
    }
}