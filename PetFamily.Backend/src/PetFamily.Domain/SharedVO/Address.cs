using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SharedVO;

public record Address
{
    public const int MAX_APARTMENT_TEXT_LENGTH = 30;
    public NotEmptyString Region { get; }
    public NotEmptyString City { get; }
    public NotEmptyString Street { get; }
    public NotEmptyString Building { get; }
    public string? Apartment { get; }
    
    private Address() { }

    private Address(
        NotEmptyString region, 
        NotEmptyString city, 
        NotEmptyString street, 
        NotEmptyString building, 
        string apartment)
    {
        Region = region;
        City = city;
        Street = street;
        Building = building;
        Apartment = apartment;
    }

    public static Result<Address> Create(
        NotEmptyString region, 
        NotEmptyString city, 
        NotEmptyString street,
        NotEmptyString building, 
        string apartment)
    {
        if (apartment.Length > MAX_APARTMENT_TEXT_LENGTH)
        {
            return $"Apartment cannot be longer than {MAX_APARTMENT_TEXT_LENGTH} characters.";
        }
        
        return new Address(region, city, street, building, apartment);
    }
}