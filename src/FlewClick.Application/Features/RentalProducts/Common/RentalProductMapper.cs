using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.RentalProducts.Common;

public static class RentalProductMapper
{
    public static RentalProductDto ToDto(RentalProduct product) =>
        new(
            product.Id,
            product.RentalStoreId,
            product.Name,
            product.Description,
            product.Category,
            product.Brand,
            product.Model,
            product.Condition,
            product.Specifications,
            product.IsAvailable,
            product.CreatedAtUtc,
            product.UpdatedAtUtc
        );

    public static RentalProductImageDto ToDto(RentalProductImage image) =>
        new(image.Id, image.RentalProductId, image.ImageUrl, image.DisplayOrder, image.IsPrimary);

    public static RentalProductPricingDto ToDto(RentalProductPricing pricing) =>
        new(pricing.Id, pricing.RentalProductId, pricing.HourlyRate,
            pricing.DailyRate, pricing.WeeklyRate, pricing.MonthlyRate, pricing.DepositAmount);
}
