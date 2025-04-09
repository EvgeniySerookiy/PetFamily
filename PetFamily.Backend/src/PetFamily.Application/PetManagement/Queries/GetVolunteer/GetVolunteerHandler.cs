using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteer;

public class GetVolunteerHandler : IQueryHandlerVolunteer<PagedList<VolunteerDto>, GetVolunteerQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteerHandler(
        IReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _context.Volunteers;
        
        var volunteer = await volunteersQuery
            .Where(v => v.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (volunteer == null)
            
            return Errors.General.NotFound(query.Id).ToErrorList();

        return new VolunteerDto
        {
            Id = volunteer.Id,
            FirstName = volunteer.FirstName,
            LastName = volunteer.LastName,
            Email = volunteer.Email,
            PhoneNumber = volunteer.PhoneNumber,
        };
    }
}