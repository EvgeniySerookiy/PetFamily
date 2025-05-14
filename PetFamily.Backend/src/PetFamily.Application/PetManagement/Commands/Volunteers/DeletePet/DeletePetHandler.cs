using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.DeletePet;

public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly IVolunteersReadRepository _volunteersReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        IVolunteersReadRepository volunteersReadRepository,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _volunteersReadRepository = volunteersReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var petExist = await _volunteersReadRepository
            .CheckWithVolunteerFoarAPet(command.VolunteerId, command.PetId, cancellationToken);
        if (petExist.IsFailure)
            return petExist.Error.ToErrorList();
        
        var volunteerResult = await _volunteersWriteRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        petResult.Value.Delete();
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return command.PetId;
    }
}