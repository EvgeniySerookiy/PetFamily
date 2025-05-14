using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateMainInfo;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Volunteers.UpdateVolunteer;

public class UpdateMainInfoTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, UpdateMainInfoCommand> _sut;

    public UpdateMainInfoTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
    }

    [Fact]
    public async Task Update_Main_Info_To_Database_Succeeds()
    {
        // Arrange
        var createVolunteer = SharedTestsSeeder.CreateVolunteer();
        await VolunteersWriteRepository.Add(createVolunteer);

        var command = new UpdateMainInfoCommand(createVolunteer.Id, CreateMainInfoDto());
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(result.Value);
        
        var updateVolunteer = await ReadDbContext.Volunteers.FirstOrDefaultAsync();

        updateVolunteer.Should().NotBeNull();
        updateVolunteer.FirstName.Should().Be(CreateFullNameDto().FirstName);
        updateVolunteer.PhoneNumber.Should().Be(CreateMainInfoDto().PhoneNumber);
        updateVolunteer.Id.Should().Be(result.Value);
    }
    
    [Fact]
    public async Task Update_Main_Info_To_Database_When_Volunteer_Not_Found_Fails()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId().Value;
        var command = new UpdateMainInfoCommand(volunteerId, CreateMainInfoDto());
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(volunteerId));
    }
    

    private MainInfoDto CreateMainInfoDto()
    {
        return new MainInfoDto(
            CreateFullNameDto(),
            "newegor@gmail.com",
            "Новое описание",
            2,
            "+375446785689");
    }

    private FullNameDto CreateFullNameDto()
    {
        return new FullNameDto(
            "Егор",
            "Казанович",
            "Алексеевич");
    }
}