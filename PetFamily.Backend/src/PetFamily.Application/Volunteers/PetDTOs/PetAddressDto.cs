namespace PetFamily.Application.Volunteers.DTOs;

public record PetAddressDto(
    string Region,
    string City,
    string Street,
    string Building,
    string? Apartment);