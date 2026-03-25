using FlewClick.Application.Features.ProfessionalDetail.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalDetail.GetProfessionalDetail;

public record GetProfessionalDetailQuery(Guid ProfileId) : IRequest<ProfessionalDetailDto>;

public class GetProfessionalDetailValidator : AbstractValidator<GetProfessionalDetailQuery>
{
    public GetProfessionalDetailValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class GetProfessionalDetailHandler(
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IPackageRepository packageRepository,
    IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<GetProfessionalDetailQuery, ProfessionalDetailDto>
{
    public async Task<ProfessionalDetailDto> Handle(GetProfessionalDetailQuery request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        var packages = await packageRepository.GetByProfileIdAsync(request.ProfileId, ct);
        var activePackageCount = packages.Count(p => p.IsActive);

        var portfolioItems = await portfolioItemRepository.GetByProfileIdAsync(
            request.ProfileId, visibleOnly: true, ct: ct);

        return new ProfessionalDetailDto(
            profile.Id,
            user.FullName,
            profile.Bio,
            profile.Location,
            profile.YearsOfExperience,
            profile.HourlyRate,
            profile.PortfolioUrl,
            user.ProfessionalRoles,
            activePackageCount,
            portfolioItems.Count,
            profile.CreatedAtUtc
        );
    }
}
