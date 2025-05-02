using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.RestoreVolunteer;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Volunteers;

public class RestoreVolunteerTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, RestoreVolunteerCommand> _sut;

    public RestoreVolunteerTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, RestoreVolunteerCommand>>();
    }

    [Fact]
    public async Task Restore_Volunteer_To_Database_Succeeds()
    {
        // Arrange
        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersRepository.Add(createVolunteer);

        var command = new RestoreVolunteerCommand(createVolunteer.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deletedVolunteer = await ReadDbContext.Volunteers
            .FirstOrDefaultAsync(d => d.Id == result.Value);

        deletedVolunteer.Should().NotBeNull();
        deletedVolunteer.IsDeleted.Should().BeFalse();
    }
    
    [Fact]
    public async Task Restore_Volunteer_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var command = new RestoreVolunteerCommand(volunteerId);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }
}