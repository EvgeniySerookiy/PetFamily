using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Restore;

public class RestoreVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<RestoreVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<RestoreVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Restore();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Restored volunteer with id {volunteerId}", id);

        return volunteerResult.Value.Id.Value;
    }
}