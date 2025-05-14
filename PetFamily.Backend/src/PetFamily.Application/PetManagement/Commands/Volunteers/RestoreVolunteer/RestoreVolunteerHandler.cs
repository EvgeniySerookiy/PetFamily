using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.RestoreVolunteer;

public class RestoreVolunteerHandler : ICommandHandler<Guid, RestoreVolunteerCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<RestoreVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreVolunteerHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<RestoreVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        RestoreVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersWriteRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Restore();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Restored volunteer with id {volunteerId}", command.Id);

        return volunteerResult.Value.Id.Value;
    }
}