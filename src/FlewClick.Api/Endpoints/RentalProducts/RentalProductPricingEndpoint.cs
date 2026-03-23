using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Features.RentalProducts.SetProductPricing;
using MediatR;

namespace FlewClick.Api.Endpoints.RentalProducts;

public class RentalProductPricingEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rental/products/{productId:guid}/pricing",
                async (IMediator mediator, Guid productId, SetProductPricingRequest request) =>
                    Results.Ok(await mediator.Send(new SetProductPricingCommand(
                        productId, request.DepositAmount, request.HourlyRate,
                        request.DailyRate, request.WeeklyRate, request.MonthlyRate))))
            .WithName("SetProductPricing")
            .WithTags("RentalProducts")
            .Produces<RentalProductPricingDto>()
            .Produces(400);
    }
}

public record SetProductPricingRequest(
    decimal DepositAmount,
    decimal? HourlyRate,
    decimal? DailyRate,
    decimal? WeeklyRate,
    decimal? MonthlyRate);
