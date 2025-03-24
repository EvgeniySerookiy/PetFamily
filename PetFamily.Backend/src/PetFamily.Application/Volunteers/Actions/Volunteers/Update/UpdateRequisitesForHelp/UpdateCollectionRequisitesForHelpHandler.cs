using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public class UpdateCollectionRequisitesForHelpHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCollectionRequisitesForHelpHandler> _logger;

    public UpdateCollectionRequisitesForHelpHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCollectionRequisitesForHelpHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateCollectionRequisitesForHelpRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisitesForHelps = request.CollectionRequisitesForHelp.RequisitesForHelps;
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
            request.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}