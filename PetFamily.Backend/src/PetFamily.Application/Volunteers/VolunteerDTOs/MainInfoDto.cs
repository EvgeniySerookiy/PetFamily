namespace PetFamily.Application.Volunteers.DTOs;

public record MainInfoDto(
    FullNameDto FullName,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber);