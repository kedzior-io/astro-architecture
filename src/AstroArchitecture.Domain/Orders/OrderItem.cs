using Ardalis.GuardClauses;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class OrderItem: Entity<int>, IValueObject
{
    public int ProductId { get; private set; }
     public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal Total => Quantity * Price;

    public OrderItem(Product product, int quantity)
    {
        Guard.Against.NegativeOrZero(quantity);

        if(product.Stock < quantity) 
        {
            throw new Exception("Product is out of stock");
        }

        Quantity = quantity;
        Price =  product.Price;

        /* 
            Feel free to add more product related properties such image.
            It's important to clone these in case product is renamed or its price changed.
        */

        ProductId = product.Id;
        ProductName = product.Name;
    }

}