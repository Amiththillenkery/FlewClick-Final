using FlewClick.Application.Features.Browse.SearchProfessionals;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Browse;

public class SearchEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/search/professionals", async (
                IMediator mediator,
                string q,
                ProfessionalRole? role,
                int page = 1,
                int pageSize = 20) =>
            {
                var result = await mediator.Send(new SearchProfessionalsQuery(q, role, page, pageSize));
                return Results.Ok(result);
            })
            .WithName("SearchProfessionals")
            .WithTags("Search")
            .AllowAnonymous();
    }
}
