namespace PetFamily.Application.Dtos.PetDTOs;

public record PetAddressDto(
    string Region,
    string City,
    string Street,
    string Building,
    string? Apartment);