using AstroArchitecture.Core.Enums;

namespace AstroArchitecture.Handlers;

public sealed class OrderListModel(Order order)
{
    public string CustomerName { get; private set; } = order.Customer.FullName;
    public int NumberOfItems { get; private set; } = order.Items.Count;
    public string Country { get; private set; } = order.Address.Country;
    public OrderStatus Status { get; private set; } = order.OrderStatus;

    public static OrderListModel Create(Order order)
    {
        return new OrderListModel(order);
    }
}