namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public bool IsDeleted { get; init; }
    
}