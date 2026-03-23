using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Features.Users.GetUserById;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class GetUserByIdEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new GetUserByIdQuery(id));
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .WithName("GetUserById")
            .WithTags("Users")
            .Produces<AppUserDto>()
            .Produces(404);
    }
}
