using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Features.Users.GetUsersByType;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class GetUsersByTypeEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/type/{userType}", async (IMediator mediator, UserType userType) =>
                Results.Ok(await mediator.Send(new GetUsersByTypeQuery(userType))))
            .WithName("GetUsersByType")
            .WithTags("Users")
            .Produces<IReadOnlyList<AppUserDto>>();
    }
}
