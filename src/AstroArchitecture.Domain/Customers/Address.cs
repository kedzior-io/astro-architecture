using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Address : Entity<Guid>, IValueObject
{
    public string Name { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    public virtual Customer Customer { get; private set; }

    private Address()
    {
        // EF
    }

    public Address(string name, string street, string city, string country, string zipCode)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(street);
        Guard.Against.NullOrWhiteSpace(country);
        Guard.Against.NullOrWhiteSpace(zipCode);

        Name = name;
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
    }
}