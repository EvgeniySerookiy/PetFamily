using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public class UpdateRequisitesForHelpHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateRequisitesForHelpHandler> _logger;
    private readonly IValidator<UpdateRequisitesForHelpCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRequisitesForHelpHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateRequisitesForHelpHandler> logger,
        IValidator<UpdateRequisitesForHelpCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        Guid id,
        UpdateRequisitesForHelpCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);
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
            id);

        return volunteerResult.Value.Id.Value;
    }
}