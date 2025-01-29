namespace Maat.Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    
    private Address() { }

    public Address(string street, string city, string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        
        return Street == other.Street && 
               City == other.City && 
               PostalCode == other.PostalCode && 
               Country == other.Country;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, PostalCode, Country);
    }
}