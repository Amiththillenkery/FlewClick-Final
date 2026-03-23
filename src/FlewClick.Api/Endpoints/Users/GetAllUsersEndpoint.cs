using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Features.Users.GetAllUsers;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class GetAllUsersEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (IMediator mediator) =>
                Results.Ok(await mediator.Send(new GetAllUsersQuery())))
            .WithName("GetAllUsers")
            .WithTags("Users")
            .Produces<IReadOnlyList<AppUserDto>>();
    }
}
