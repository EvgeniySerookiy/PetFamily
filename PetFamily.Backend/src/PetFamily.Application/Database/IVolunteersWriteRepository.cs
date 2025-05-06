using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Database;

public interface IVolunteersWriteRepository
{
    Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default);
    
    Task<Guid> Delete(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByEmail(
        Email email,
        CancellationToken cancellationToken = default);
}