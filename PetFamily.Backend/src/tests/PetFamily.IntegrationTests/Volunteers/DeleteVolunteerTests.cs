using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.DeleteVolunteer;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Volunteers;

public class DeleteVolunteerTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommandHandler;

    public DeleteVolunteerTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
        
        _createVolunteerCommandHandler = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Delete_Volunteer_To_Database_Succeeds()
    {
        // Arrange
        var createVolunteerCommand = Fixture.CreateVolunteerCommand();
        var createVolunteer = await _createVolunteerCommandHandler.Handle(createVolunteerCommand);
        
        var command = new DeleteVolunteerCommand(createVolunteer.Value);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var deletedVolunteer = await ReadDbContext.Volunteers
            .FirstOrDefaultAsync(d => d.Id == result.Value);
        
        deletedVolunteer.Should().NotBeNull();
        deletedVolunteer.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Volunteer_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var command = new DeleteVolunteerCommand(volunteerId);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }
}