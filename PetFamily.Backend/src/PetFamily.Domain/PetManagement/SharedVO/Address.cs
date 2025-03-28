using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.SharedVO;

public record Address
{
    public const int MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH = 30;
    public string Region { get; }
    public string City { get; }
    public string Street { get; }
    public string Building { get; }
    public string? Apartment { get; }

    private Address(
        string region, 
        string city, 
        string street, 
        string building, 
        string? apartment = null)
    {
        Region = region;
        City = city;
        Street = street;
        Building = building;
        Apartment = apartment;
    }

    public static Result<Address, Error> Create(
        string region, 
        string city, 
        string street,
        string building, 
        string apartment)
    {
        if (string.IsNullOrWhiteSpace(region))
            return Errors.General.ValueIsRequired("Region");
        
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsRequired("City");
        
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsRequired("Street");
        
        if (string.IsNullOrWhiteSpace(building))
            return Errors.General.ValueIsRequired("Building");
        
        if (region.Length > MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Region", MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH);
        
        if (city.Length > MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("City", MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH);
        
        if (street.Length > MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Street", MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH);
            
        if (building.Length > MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Building", MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH);
        
        if (apartment.Length > MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Apartment", MAX_ALL_FIELDS_ADDRESS_TEXT_LENGTH);
        
        return new Address(region, city, street, building, apartment);
    }
}