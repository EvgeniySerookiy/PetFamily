using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository
{
    private readonly ApplicationDbContex _applicationDbContex;

    public VolunteersRepository(ApplicationDbContex applicationDbContex)
    {
        _applicationDbContex = applicationDbContex;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken)
    {
        await _applicationDbContex.Volunteers.AddAsync(volunteer, cancellationToken);
        await _applicationDbContex.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer>> GetById(VolunteerId volunteerId)
    {
        var volunteer = await _applicationDbContex.Volunteers
            .Include(m => m.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer is null)
        {
            return "Volunteer not found";
        }

        return volunteer;
    }

}