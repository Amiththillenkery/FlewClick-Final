using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class PackagePricing : Entity
{
    public Guid PackageId { get; private init; }
    public PricingType PricingType { get; private set; }
    public decimal BasePrice { get; private set; }
    public decimal? DiscountPercentage { get; private set; }
    public decimal FinalPrice { get; private set; }
    public int? DurationHours { get; private set; }
    public bool IsNegotiable { get; private set; }
    public string? Notes { get; private set; }

    private PackagePricing() { }

    public static PackagePricing Create(
        Guid packageId,
        PricingType pricingType,
        decimal basePrice,
        decimal? discountPercentage = null,
        int? durationHours = null,
        bool isNegotiable = false,
        string? notes = null)
    {
        if (packageId == Guid.Empty)
            throw new DomainException("Package ID is required.");

        if (basePrice < 0)
            throw new DomainException("Base price cannot be negative.");

        if (discountPercentage is < 0 or > 100)
            throw new DomainException("Discount percentage must be between 0 and 100.");

        if (durationHours is <= 0)
            throw new DomainException("Duration hours must be positive.");

        var finalPrice = CalculateFinalPrice(basePrice, discountPercentage);

        return new PackagePricing
        {
            PackageId = packageId,
            PricingType = pricingType,
            BasePrice = basePrice,
            DiscountPercentage = discountPercentage,
            FinalPrice = finalPrice,
            DurationHours = durationHours,
            IsNegotiable = isNegotiable,
            Notes = notes?.Trim()
        };
    }

    public void Update(PricingType pricingType, decimal basePrice, decimal? discountPercentage,
        int? durationHours, bool isNegotiable, string? notes)
    {
        if (basePrice < 0)
            throw new DomainException("Base price cannot be negative.");

        if (discountPercentage is < 0 or > 100)
            throw new DomainException("Discount percentage must be between 0 and 100.");

        if (durationHours is <= 0)
            throw new DomainException("Duration hours must be positive.");

        PricingType = pricingType;
        BasePrice = basePrice;
        DiscountPercentage = discountPercentage;
        FinalPrice = CalculateFinalPrice(basePrice, discountPercentage);
        DurationHours = durationHours;
        IsNegotiable = isNegotiable;
        Notes = notes?.Trim();
        Touch();
    }

    private static decimal CalculateFinalPrice(decimal basePrice, decimal? discountPercentage) =>
        discountPercentage.HasValue
            ? Math.Round(basePrice * (1 - discountPercentage.Value / 100m), 2)
            : basePrice;
}
