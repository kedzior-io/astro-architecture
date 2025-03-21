namespace AstroArchitecture.Handlers.Handlers.Addresses.Models;

public sealed class AddressListModel(Address address)
{
    public Guid Id { get; private set; } = address.Id;
    public string Name { get; private set; } = address.Name;

    public static AddressListModel Create(Address address)
    {
        return new AddressListModel(address);
    }
}