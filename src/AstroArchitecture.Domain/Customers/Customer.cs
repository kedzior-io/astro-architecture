using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Customer : Entity<Guid>, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public virtual ICollection<Address> Addresses { get; private set; } = new List<Address>();
    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();
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

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}