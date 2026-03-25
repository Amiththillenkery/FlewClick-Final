using FlewClick.Application.Features.Browse.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.Browse.GetCategories;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesHandler(IBrowseRepository browseRepository)
    : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var counts = await browseRepository.GetCategoryCountsAsync(ct);

        return counts.Select(c => new CategoryDto(
            c.Role,
            c.Role.ToString(),
            c.Count
        )).ToList();
    }
}
