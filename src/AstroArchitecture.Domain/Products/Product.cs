using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Product : Entity<int>, IAggregateRoot
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; } = 0;

    public Product(string name, decimal price, int stock)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NegativeOrZero(price);

        Name = name;
        Price = price;
        Stock = stock;
    }
}