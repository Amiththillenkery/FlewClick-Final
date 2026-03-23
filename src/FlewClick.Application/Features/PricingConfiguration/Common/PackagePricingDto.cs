using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.PricingConfiguration.Common;

public record PackagePricingDto(
    Guid Id,
    Guid PackageId,
    PricingType PricingType,
    decimal BasePrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    int? DurationHours,
    bool IsNegotiable,
    string? Notes
);
