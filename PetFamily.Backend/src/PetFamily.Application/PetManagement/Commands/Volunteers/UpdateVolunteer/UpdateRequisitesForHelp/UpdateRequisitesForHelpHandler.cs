using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;

public class UpdateRequisitesForHelpHandler : ICommandHandler<Guid, UpdateRequisitesForHelpCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<UpdateRequisitesForHelpHandler> _logger;
    private readonly IValidator<UpdateRequisitesForHelpCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRequisitesForHelpHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<UpdateRequisitesForHelpHandler> logger,
        IValidator<UpdateRequisitesForHelpCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesForHelpCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersWriteRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var requisitesForHelps = command.RequisitesForHelps;
        var requisitesForHelpList = new List<RequisitesForHelp>();
        foreach (var requisitesForHelp in requisitesForHelps)
        {
            var value = RequisitesForHelp.Create(
                requisitesForHelp.Recipient,
                requisitesForHelp.PaymentDetails).Value;

            requisitesForHelpList.Add(value);
        }

        volunteerResult.Value.UpdateRequisitesForHelp(
            requisitesForHelpList);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer {requisitesForHelpList} with id {volunteerId}", 
            requisitesForHelpList,
            command.Id);

        return volunteerResult.Value.Id.Value;
    }
}