using AstroArchitecture.Core.Enums;

namespace AstroArchitecture.Handlers;

public sealed class OrderListModel(Order order)
{
    public string CustomerName { get; private set; } = order.Customer.FullName;
    public OrderStatus Status { get; private set; } = order.OrderStatus;

    public static OrderListModel Create(Order order)
    {
        return new OrderListModel(order);
    }
}