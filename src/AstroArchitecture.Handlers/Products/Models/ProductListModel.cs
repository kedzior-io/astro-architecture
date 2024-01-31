namespace AstroArchitecture.Handlers.Customers;

public sealed class ProductListModel(Product product)
{
    public int Id { get; private set; } = product.Id;
    public string Name { get; private set; } = product.Name;

    public static ProductListModel Create(Product product)
    {
        return new ProductListModel(product);
    }
}