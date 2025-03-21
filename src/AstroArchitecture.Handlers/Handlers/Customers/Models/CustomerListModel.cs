namespace AstroArchitecture.Handlers.Handlers.Customers.Models;

public sealed class CustomerListModel(Customer customer)
{
    public Guid Id { get; private set; } = customer.Id;
    public string FullName { get; private set; } = customer.FullName;

    public static CustomerListModel Create(Customer customer)
    {
        return new CustomerListModel(customer);
    }
}