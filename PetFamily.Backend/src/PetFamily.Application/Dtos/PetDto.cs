using PetFamily.Domain.PetManagement.PetVO;

namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string PetName { get; init; } = string.Empty;
    public int Position { get; init; }
}