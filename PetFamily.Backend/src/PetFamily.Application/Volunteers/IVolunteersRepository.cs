using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId);
    
    Task<Result<Volunteer, Error>> GetByEmail(Email email);
}