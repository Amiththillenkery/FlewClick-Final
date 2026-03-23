using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Features.Users.GetProfessionalsByRole;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class GetProfessionalsByRoleEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/professionals/{role}", async (IMediator mediator, ProfessionalRole role) =>
                Results.Ok(await mediator.Send(new GetProfessionalsByRoleQuery(role))))
            .WithName("GetProfessionalsByRole")
            .WithTags("Users")
            .Produces<IReadOnlyList<AppUserDto>>();
    }
}
