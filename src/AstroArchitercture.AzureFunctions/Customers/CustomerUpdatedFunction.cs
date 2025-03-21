using AstroArchitecture.Handlers.Handlers.Customers.Commands;
using System.Threading.Tasks;

namespace AstroArchitercture.AzureFunctions.Customers;

public class CustomerUpdatedFunction
{
    [Function(nameof(CustomerUpdatedFunction))]
    public async Task Run([ServiceBusTrigger("customer-updated", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
    {
        await AzureFunction.ExecuteServiceBusAsync<CustomerUpdated.Command>(json, CustomJsonOptions.Defaults, context);
    }
}