using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Requests;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Update;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if(volunteerResult.IsFailure)
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
        
        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        return result;
    }
}