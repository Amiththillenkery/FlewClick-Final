using FlewClick.Application.Features.RentalProducts.AddProduct;
using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Features.RentalProducts.DeleteProduct;
using FlewClick.Application.Features.RentalProducts.GetProductById;
using FlewClick.Application.Features.RentalProducts.ListProducts;
using FlewClick.Application.Features.RentalProducts.ToggleAvailability;
using FlewClick.Application.Features.RentalProducts.UpdateProduct;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.RentalProducts;

public class RentalProductEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rental/store/{storeId:guid}/products", async (IMediator mediator, Guid storeId) =>
                Results.Ok(await mediator.Send(new ListProductsQuery(storeId))))
            .WithName("ListRentalProducts")
            .WithTags("RentalProducts")
            .Produces<IReadOnlyList<RentalProductDto>>();

        app.MapPost("/api/rental/store/{storeId:guid}/products",
                async (IMediator mediator, Guid storeId, AddProductRequest request) =>
                {
                    var result = await mediator.Send(new AddProductCommand(
                        storeId, request.Name, request.Condition,
                        request.Description, request.Category, request.Brand,
                        request.Model, request.Specifications));
                    return Results.Created($"/api/rental/products/{result.Id}", result);
                })
            .WithName("AddRentalProduct")
            .WithTags("RentalProducts")
            .Produces<RentalProductDto>(201)
            .Produces(400);

        app.MapGet("/api/rental/products/{id:guid}", async (IMediator mediator, Guid id) =>
                Results.Ok(await mediator.Send(new GetProductByIdQuery(id))))
            .WithName("GetRentalProductById")
            .WithTags("RentalProducts")
            .Produces<RentalProductDetailDto>()
            .Produces(404);

        app.MapPut("/api/rental/products/{id:guid}",
                async (IMediator mediator, Guid id, UpdateProductRequest request) =>
                    Results.Ok(await mediator.Send(new UpdateProductCommand(
                        id, request.Name, request.Condition, request.Description,
                        request.Category, request.Brand, request.Model, request.Specifications))))
            .WithName("UpdateRentalProduct")
            .WithTags("RentalProducts")
            .Produces<RentalProductDto>()
            .Produces(404);

        app.MapDelete("/api/rental/products/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeleteProductCommand(id));
                return Results.NoContent();
            })
            .WithName("DeleteRentalProduct")
            .WithTags("RentalProducts")
            .Produces(204)
            .Produces(404);

        app.MapPatch("/api/rental/products/{id:guid}/availability",
                async (IMediator mediator, Guid id, ToggleAvailabilityRequest request) =>
                    Results.Ok(await mediator.Send(new ToggleAvailabilityCommand(id, request.Available))))
            .WithName("ToggleProductAvailability")
            .WithTags("RentalProducts")
            .Produces<RentalProductDto>()
            .Produces(404);
    }
}

public record AddProductRequest(
    string Name,
    ProductCondition Condition,
    string? Description,
    string? Category,
    string? Brand,
    string? Model,
    Dictionary<string, object?>? Specifications);

public record UpdateProductRequest(
    string Name,
    ProductCondition Condition,
    string? Description,
    string? Category,
    string? Brand,
    string? Model,
    Dictionary<string, object?>? Specifications);

public record ToggleAvailabilityRequest(bool Available);
