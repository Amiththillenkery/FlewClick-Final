using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.PricingConfiguration.Common;

public static class PricingMapper
{
    public static PackagePricingDto ToDto(PackagePricing pricing) =>
        new(
            pricing.Id,
            pricing.PackageId,
            pricing.PricingType,
            pricing.BasePrice,
            pricing.DiscountPercentage,
            pricing.FinalPrice,
            pricing.DurationHours,
            pricing.IsNegotiable,
            pricing.Notes
        );
}
