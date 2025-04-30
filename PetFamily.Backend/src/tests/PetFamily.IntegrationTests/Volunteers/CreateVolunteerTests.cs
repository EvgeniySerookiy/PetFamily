using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;

namespace PetFamily.IntegrationTests.Volunteers;

public class CreateVolunteerTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;

    public CreateVolunteerTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Create_Volunteer_To_Database_Succeeds()
    {
        // Arrange
        var command = Fixture.CreateVolunteerCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);
        
        var addedVolunteer = await ReadDbContext.Volunteers.FirstOrDefaultAsync();

        addedVolunteer.Should().NotBeNull();
        addedVolunteer.Id.Should().Be(result.Value);
    }
}