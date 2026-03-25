using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.ProfessionalDetail.Common;

public record ProfessionalDetailDto(
    Guid ProfileId,
    string FullName,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl,
    List<ProfessionalRole> ProfessionalRoles,
    int PackageCount,
    int PortfolioItemCount,
    DateTime CreatedAtUtc
);

public record PackageDetailDto(
    Guid Id,
    string Name,
    string? Description,
    PackageType PackageType,
    CoverageType? CoverageType,
    bool IsActive,
    PackagePricingInfoDto? Pricing,
    List<PackageDeliverableInfoDto> Deliverables
);

public record PackagePricingInfoDto(
    PricingType PricingType,
    decimal BasePrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    int? DurationHours,
    bool IsNegotiable,
    string? Notes
);

public record PackageDeliverableInfoDto(
    Guid Id,
    string DeliverableName,
    int Quantity,
    Dictionary<string, object?> Configuration,
    string? Notes
);
