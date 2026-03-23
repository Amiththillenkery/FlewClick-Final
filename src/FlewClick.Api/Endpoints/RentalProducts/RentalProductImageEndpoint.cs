using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Features.RentalProducts.RemoveProductImage;
using FlewClick.Application.Features.RentalProducts.UploadProductImage;
using MediatR;

namespace FlewClick.Api.Endpoints.RentalProducts;

public class RentalProductImageEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rental/products/{productId:guid}/images",
                async (IMediator mediator, Guid productId, UploadImageRequest request) =>
                {
                    var result = await mediator.Send(new UploadProductImageCommand(
                        productId, request.ImageUrl, request.DisplayOrder, request.IsPrimary));
                    return Results.Created($"/api/rental/products/{productId}", result);
                })
            .WithName("UploadProductImage")
            .WithTags("RentalProducts")
            .Produces<RentalProductImageDto>(201)
            .Produces(400);

        app.MapDelete("/api/rental/products/images/{imageId:guid}", async (IMediator mediator, Guid imageId) =>
            {
                await mediator.Send(new RemoveProductImageCommand(imageId));
                return Results.NoContent();
            })
            .WithName("RemoveProductImage")
            .WithTags("RentalProducts")
            .Produces(204)
            .Produces(404);
    }
}

public record UploadImageRequest(string ImageUrl, int DisplayOrder, bool IsPrimary);
