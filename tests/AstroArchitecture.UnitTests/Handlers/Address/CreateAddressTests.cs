using AstroArchitecture.Handlers.Handlers.Addresses.Commands;
using AstroArchitecture.Handlers.Handlers.Products.Commands;

namespace AstroArchitecture.UnitTests.Handlers.Orders;

public class CreateAddressTests
{
    private static CreateAddress.Handler GetHandler(ApplicationDbContext dbContext)
    {
        return new CreateAddress.Handler(HandlerContextFactory.GetHandlerContext(dbContext));
    }

    private static void SetTestData(ApplicationDbContext dc)
    {
        var customer = new Customer("Doom", "Slayer", "doomslayer@fortress-of-doom.com");

        dc.Customers.AddRange(customer);
        dc.SaveChanges();
    }

    [Fact]
    public async Task Create_Address_PublishEvent()
    {
        var dc = InMemoryDbContextFactory.Create();
        SetTestData(dc);

        var customer = dc.Customers.First();

        var command = new CreateAddress.Command(customer.Id, "Fortress of Doom", "Fortress of Doom", "Fortress of Doom", "Fortress of Doom", "Fortress of Doom");

        var response = await GetHandler(dc).ExecuteAsync(command, default);

        Assert.NotNull(response);
    }
}