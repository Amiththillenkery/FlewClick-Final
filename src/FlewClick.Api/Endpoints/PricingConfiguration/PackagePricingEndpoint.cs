using FlewClick.Application.Features.PricingConfiguration.Common;
using FlewClick.Application.Features.PricingConfiguration.GetPackagePricing;
using FlewClick.Application.Features.PricingConfiguration.RemovePackagePricing;
using FlewClick.Application.Features.PricingConfiguration.SetPackagePricing;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.PricingConfiguration;

public class PackagePricingEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/packages/{packageId:guid}/pricing", async (IMediator mediator, Guid packageId) =>
            {
                var result = await mediator.Send(new GetPackagePricingQuery(packageId));
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName("GetPackagePricing")
            .WithTags("Pricing")
            .Produces<PackagePricingDto>()
            .Produces(404);

        app.MapPost("/api/packages/{packageId:guid}/pricing",
                async (IMediator mediator, Guid packageId, SetPricingRequest request) =>
                    Results.Ok(await mediator.Send(new SetPackagePricingCommand(
                        packageId, request.PricingType, request.BasePrice,
                        request.DiscountPercentage, request.DurationHours,
                        request.IsNegotiable, request.Notes))))
            .WithName("SetPackagePricing")
            .WithTags("Pricing")
            .Produces<PackagePricingDto>()
            .Produces(400);

        app.MapDelete("/api/packages/{packageId:guid}/pricing", async (IMediator mediator, Guid packageId) =>
            {
                await mediator.Send(new RemovePackagePricingCommand(packageId));
                return Results.NoContent();
            })
            .WithName("RemovePackagePricing")
            .WithTags("Pricing")
            .Produces(204)
            .Produces(404);
    }
}

public record SetPricingRequest(
    PricingType PricingType,
    decimal BasePrice,
    decimal? DiscountPercentage,
    int? DurationHours,
    bool IsNegotiable,
    string? Notes);
