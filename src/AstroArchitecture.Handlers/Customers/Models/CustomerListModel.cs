namespace AstroArchitecture.Handlers;

public sealed class CustomerListModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }

    public CustomerListModel()
    {
    }

    public CustomerListModel(Customer customer)
    {
        Id = customer.Id;
        FullName = customer.FullName;
    }

    public static CustomerListModel Create(Customer customer)
    {
        return new CustomerListModel(customer);
    }
}