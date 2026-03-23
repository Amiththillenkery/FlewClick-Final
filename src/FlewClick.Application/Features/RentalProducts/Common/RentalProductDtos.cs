using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.RentalProducts.Common;

public record RentalProductDto(
    Guid Id,
    Guid RentalStoreId,
    string Name,
    string? Description,
    string? Category,
    string? Brand,
    string? Model,
    ProductCondition Condition,
    Dictionary<string, object?> Specifications,
    bool IsAvailable,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record RentalProductImageDto(
    Guid Id,
    Guid RentalProductId,
    string ImageUrl,
    int DisplayOrder,
    bool IsPrimary
);

public record RentalProductPricingDto(
    Guid Id,
    Guid RentalProductId,
    decimal? HourlyRate,
    decimal? DailyRate,
    decimal? WeeklyRate,
    decimal? MonthlyRate,
    decimal DepositAmount
);

public record RentalProductDetailDto(
    RentalProductDto Product,
    IReadOnlyList<RentalProductImageDto> Images,
    RentalProductPricingDto? Pricing
);
