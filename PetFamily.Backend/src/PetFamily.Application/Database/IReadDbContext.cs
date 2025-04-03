using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public DbSet<VolunteerDto> Volunteers { get; }
    public DbSet<PetDto> Pets { get; }
}