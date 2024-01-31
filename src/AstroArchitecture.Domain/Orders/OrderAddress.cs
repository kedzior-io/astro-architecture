using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class OrderAddress: IValueObject
{
    public string Recepient { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    public OrderAddress(string recepient, string street, string city, string country, string zipCode)
    {
        Guard.Against.NullOrWhiteSpace(recepient);
        Guard.Against.NullOrWhiteSpace(country);
        Guard.Against.NullOrWhiteSpace(zipCode);

        Recepient = recepient;
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
    }

    public static OrderAddress Create(Address address, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(address);

        return new OrderAddress(
            customer.FullName,
            address.Street,
            address.City,
            address.Country,
            address.ZipCode);
    }
}