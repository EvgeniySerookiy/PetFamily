namespace PetFamily.Application.Volunteers.DTOs;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber);