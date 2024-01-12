using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Customer : Entity<int>, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public virtual ICollection<Address> Addresses { get; private set; } = new List<Address>();

    public string FullName => $"{FirstName} {LastName}";

    private Customer()
    {
        // EF
    }     

    public Customer(string firstName, string lastName, string email)
    {
        Guard.Against.NullOrWhiteSpace(firstName);
        Guard.Against.NullOrWhiteSpace(lastName);
        Guard.Against.NullOrWhiteSpace(email);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}