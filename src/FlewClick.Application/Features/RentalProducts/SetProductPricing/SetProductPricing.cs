using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.SetProductPricing;

public record SetProductPricingCommand(
    Guid ProductId,
    decimal DepositAmount,
    decimal? HourlyRate,
    decimal? DailyRate,
    decimal? WeeklyRate,
    decimal? MonthlyRate
) : IRequest<RentalProductPricingDto>;

public class SetProductPricingValidator : AbstractValidator<SetProductPricingCommand>
{
    public SetProductPricingValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.DepositAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0).When(x => x.HourlyRate.HasValue);
        RuleFor(x => x.DailyRate).GreaterThanOrEqualTo(0).When(x => x.DailyRate.HasValue);
        RuleFor(x => x.WeeklyRate).GreaterThanOrEqualTo(0).When(x => x.WeeklyRate.HasValue);
        RuleFor(x => x.MonthlyRate).GreaterThanOrEqualTo(0).When(x => x.MonthlyRate.HasValue);
    }
}

public class SetProductPricingHandler(
    IRentalProductRepository productRepository,
    IRentalProductPricingRepository pricingRepository)
    : IRequestHandler<SetProductPricingCommand, RentalProductPricingDto>
{
    public async Task<RentalProductPricingDto> Handle(SetProductPricingCommand request, CancellationToken ct)
    {
        _ = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        var existing = await pricingRepository.GetByProductIdAsync(request.ProductId, ct);

        if (existing is not null)
        {
            existing.Update(request.DepositAmount, request.HourlyRate,
                request.DailyRate, request.WeeklyRate, request.MonthlyRate);
            await pricingRepository.UpdateAsync(existing, ct);
            return RentalProductMapper.ToDto(existing);
        }

        var pricing = RentalProductPricing.Create(
            request.ProductId, request.DepositAmount,
            request.HourlyRate, request.DailyRate, request.WeeklyRate, request.MonthlyRate);

        await pricingRepository.AddAsync(pricing, ct);
        return RentalProductMapper.ToDto(pricing);
    }
}
