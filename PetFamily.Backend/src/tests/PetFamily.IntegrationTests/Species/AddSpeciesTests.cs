using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.IntegrationTests.Species;

public class AddSpeciesTests : ManagementBaseTests
{
    private readonly ICommandHandler<Guid, AddSpeciesCommand> _sut;

    public AddSpeciesTests(
        IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddSpeciesCommand>>();
    }

    [Fact]
    public async Task Add_Species_To_Database_Succeeds()
    {
        // Arrange
        var command = Fixture.CreateAddSpeciesCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var addedSpecies = await ReadDbContext.Species.FirstOrDefaultAsync();

        addedSpecies.Should().NotBeNull();
        addedSpecies.Id.Should().Be(result.Value);
    }

    [Fact]
    public async Task Add_Species_To_Database_With_EmptyName_Returns_Validation_Error()
    {
        // Arrange
        var commnd = Fixture.Build<AddSpeciesCommand>()
            .With(c => c.SpeciesName, "")
            .Create();

        // Act
        var result = await _sut.Handle(commnd, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        var expectedError = Error.Validation(
            "value.is.invalid",
            "Species name is invalid",
            "SpeciesName");

        result.Error.Should().Contain(expectedError);
    }

    [Fact]
    public async Task Add_Species_To_Database_When_Species_Already_Exist_Fails()
    {
        // Arrange
        var speciesToCreate = CreateSpecies("Species name");

        await SpeciesRepository.Add(speciesToCreate.Value);

        var command = Fixture.CreateAddSpeciesCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.Species.AlreadyExist());
    }
}