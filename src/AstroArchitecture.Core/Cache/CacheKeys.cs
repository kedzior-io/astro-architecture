namespace AstroArchitecture.Core.Cache;

public static class CacheKeys
{
    public static string CustomerAddressList(Guid customerId) => $"{customerId}-{nameof(CustomerAddressList)}";

    public static string CustomerAddress(Guid addressId) => $"{addressId}-{nameof(CustomerAddress)}";
}