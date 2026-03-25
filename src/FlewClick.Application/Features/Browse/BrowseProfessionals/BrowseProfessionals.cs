using FlewClick.Application.Features.Browse.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Browse.BrowseProfessionals;

public record BrowseProfessionalsQuery(
    ProfessionalRole? Role = null,
    string? Location = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<PaginatedResult<ProfessionalListingDto>>;

public class BrowseProfessionalsValidator : AbstractValidator<BrowseProfessionalsQuery>
{
    public BrowseProfessionalsValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class BrowseProfessionalsHandler(IBrowseRepository browseRepository)
    : IRequestHandler<BrowseProfessionalsQuery, PaginatedResult<ProfessionalListingDto>>
{
    public async Task<PaginatedResult<ProfessionalListingDto>> Handle(BrowseProfessionalsQuery request, CancellationToken ct)
    {
        var result = await browseRepository.BrowseProfessionalsAsync(
            request.Role, request.Location, request.Page, request.PageSize, ct);

        var items = result.Items.Select(i => new ProfessionalListingDto(
            i.ProfileId, i.FullName, i.Bio, i.Location,
            i.YearsOfExperience, i.HourlyRate, i.ProfessionalRoles, i.CreatedAtUtc
        )).ToList();

        return new PaginatedResult<ProfessionalListingDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
