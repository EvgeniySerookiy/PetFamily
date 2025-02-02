using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Address
{
    public string Region { get; }
    public string City { get; }
    public string Street { get; }
    public string Building { get; }
    public string Apartment { get; }

    private Address(string region, string city, string street, string building, string apartment)
    {
        Region = region;
        City = city;
        Street = street;
        Building = building;
        Apartment = apartment;
    }

    public static Result<Address> Create(string region, string city, string street, string building, string apartment)
    {
        if (string.IsNullOrWhiteSpace(region))
        {
            return Result.Failure<Address>("Region cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Failure<Address>("City cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Failure<Address>("Street cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(building))
        {
            return Result.Failure<Address>("Building cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(apartment))
        {
            return Result.Failure<Address>("Apartment cannot be empty.");
        }

        var address = new Address(region, city, street, building, apartment);
        
        return Result.Success(address);
    }
    
}