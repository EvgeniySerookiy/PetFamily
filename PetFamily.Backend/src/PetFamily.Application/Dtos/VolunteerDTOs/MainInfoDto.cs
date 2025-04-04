namespace PetFamily.Application.Dtos.VolunteerDTOs;

public record MainInfoDto(
    FullNameDto FullName,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber);