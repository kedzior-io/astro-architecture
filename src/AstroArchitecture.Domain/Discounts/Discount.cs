using Ardalis.GuardClauses;
using AstroArchitecture.Core.Extensions;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain.Discounts;
public class Discount: Entity<int>
{
    public string DiscountCode { get; private set; } = string.Empty;
    public DateTime ValidUntilUtc { get; private set; }

    public Discount(string discountCode, DateTime validUntilUtc)
    {
        Guard.Against.NullOrWhiteSpace(discountCode);
        Guard.Against.PastDate(validUntilUtc);

        DiscountCode = discountCode;
        ValidUntilUtc = validUntilUtc;
    }
}
