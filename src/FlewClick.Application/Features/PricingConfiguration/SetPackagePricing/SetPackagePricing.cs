using FlewClick.Application.Features.PricingConfiguration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PricingConfiguration.SetPackagePricing;

public record SetPackagePricingCommand(
    Guid PackageId,
    PricingType PricingType,
    decimal BasePrice,
    decimal? DiscountPercentage,
    int? DurationHours,
    bool IsNegotiable,
    string? Notes
) : IRequest<PackagePricingDto>;

public class SetPackagePricingValidator : AbstractValidator<SetPackagePricingCommand>
{
    public SetPackagePricingValidator()
    {
        RuleFor(x => x.PackageId).NotEmpty();
        RuleFor(x => x.PricingType).IsInEnum();
        RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DiscountPercentage).InclusiveBetween(0, 100).When(x => x.DiscountPercentage.HasValue);
        RuleFor(x => x.DurationHours).GreaterThan(0).When(x => x.DurationHours.HasValue);
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes is not null);
    }
}

public class SetPackagePricingHandler(
    IPackageRepository packageRepository,
    IPackagePricingRepository pricingRepository)
    : IRequestHandler<SetPackagePricingCommand, PackagePricingDto>
{
    public async Task<PackagePricingDto> Handle(SetPackagePricingCommand request, CancellationToken ct)
    {
        _ = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        var existing = await pricingRepository.GetByPackageIdAsync(request.PackageId, ct);

        if (existing is not null)
        {
            existing.Update(request.PricingType, request.BasePrice, request.DiscountPercentage,
                request.DurationHours, request.IsNegotiable, request.Notes);
            await pricingRepository.UpdateAsync(existing, ct);
            return PricingMapper.ToDto(existing);
        }

        var pricing = PackagePricing.Create(
            request.PackageId, request.PricingType, request.BasePrice,
            request.DiscountPercentage, request.DurationHours, request.IsNegotiable, request.Notes);

        await pricingRepository.AddAsync(pricing, ct);
        return PricingMapper.ToDto(pricing);
    }
}
