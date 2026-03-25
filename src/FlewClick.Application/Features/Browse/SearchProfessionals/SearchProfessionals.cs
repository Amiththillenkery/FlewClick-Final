using FlewClick.Application.Features.Browse.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Browse.SearchProfessionals;

public record SearchProfessionalsQuery(
    string Query,
    ProfessionalRole? Role = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<PaginatedResult<ProfessionalListingDto>>;

public class SearchProfessionalsValidator : AbstractValidator<SearchProfessionalsQuery>
{
    public SearchProfessionalsValidator()
    {
        RuleFor(x => x.Query).NotEmpty().MinimumLength(2).MaximumLength(200);
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class SearchProfessionalsHandler(IBrowseRepository browseRepository)
    : IRequestHandler<SearchProfessionalsQuery, PaginatedResult<ProfessionalListingDto>>
{
    public async Task<PaginatedResult<ProfessionalListingDto>> Handle(SearchProfessionalsQuery request, CancellationToken ct)
    {
        var result = await browseRepository.SearchProfessionalsAsync(
            request.Query, request.Role, request.Page, request.PageSize, ct);

        var items = result.Items.Select(i => new ProfessionalListingDto(
            i.ProfileId, i.FullName, i.Bio, i.Location,
            i.YearsOfExperience, i.HourlyRate, i.ProfessionalRoles, i.CreatedAtUtc
        )).ToList();

        return new PaginatedResult<ProfessionalListingDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
