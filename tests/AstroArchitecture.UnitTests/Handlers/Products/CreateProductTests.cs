namespace AstroArchitecture.UnitTests.Handlers.Orders;

public class CreateProductTests
{
    private static CreateProduct.Handler GetHandler(ApplicationDbContext dbContext)
    {
        return new CreateProduct.Handler(HandlerContextFactory.GetHandlerContext(dbContext));
    }

    private static void SetTestData(ApplicationDbContext dc)
    {
        var products = new List<Product> {
            new("Cyberpunk 2077", 24.2m, 100),
            new("Dune: Spice Wars", 19.99m, 100),
            new("Starcraft 2: ", 0.99m, 100),
        };

        dc.Products.AddRange(products);
        dc.SaveChanges();
    }

    [Fact]
    public async Task Create_Product_ReturnProductId()
    {
        var dc = InMemoryDbContextFactory.Create();
        SetTestData(dc);

        var productName = "Doom: Eternal";

        var command = new CreateProduct.Command(productName, 6.66m, 666);

        var expectedName = productName;
        var expectedCount = 4;

        var response = await GetHandler(dc).ExecuteAsync(command, default);

        Assert.NotNull(response?.Payload?.ProductId);

        var result = dc.Products.SingleOrDefault(o => o.Id == response.Payload.ProductId);
        var actualCount = dc.Products.Count();

        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
        Assert.Equal(expectedCount, expectedCount);
    }

    [Fact]
    public async Task Create_Product_ReturnNameTooShort()
    {
        var dc = InMemoryDbContextFactory.Create();
        SetTestData(dc);

        var command = new CreateProduct.Command("", 6.66m, 666);

        await Assert.ThrowsAsync<ArgumentException>(async () => await GetHandler(dc).ExecuteAsync(command, default));
    }
}