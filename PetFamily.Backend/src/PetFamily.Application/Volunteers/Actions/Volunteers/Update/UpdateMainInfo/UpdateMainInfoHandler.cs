using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(
            request.MainInfo.FullName.FirstName,
            request.MainInfo.FullName.LastName,
            request.MainInfo.FullName.MiddleName).Value;

        var email = Email.Create(request.MainInfo.Email).Value;
        var description = Description.Create(request.MainInfo.Description).Value;
        var yearsOfExperience = YearsOfExperience.Create(request.MainInfo.YearsOfExperience).Value;
        var phoneNumber = PhoneNumber.Create(request.MainInfo.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer {fullName}, {email}, {description}, " +
            "{yearsOfExperience}, {phoneNumber} with id {volunteerId}", 
            fullName, 
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            request.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}