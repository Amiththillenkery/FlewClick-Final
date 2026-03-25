using FlewClick.Application.Features.ProfessionalDetail.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalDetail.GetProfessionalPackages;

public record GetProfessionalPackagesQuery(Guid ProfileId) : IRequest<List<PackageDetailDto>>;

public class GetProfessionalPackagesValidator : AbstractValidator<GetProfessionalPackagesQuery>
{
    public GetProfessionalPackagesValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class GetProfessionalPackagesHandler(
    IProfessionalProfileRepository profileRepository,
    IPackageRepository packageRepository,
    IPackageDeliverableRepository deliverableRepository,
    IPackagePricingRepository pricingRepository,
    IDeliverableMasterRepository masterRepository)
    : IRequestHandler<GetProfessionalPackagesQuery, List<PackageDetailDto>>
{
    public async Task<List<PackageDetailDto>> Handle(GetProfessionalPackagesQuery request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var packages = await packageRepository.GetByProfileIdAsync(request.ProfileId, ct);
        var activePackages = packages.Where(p => p.IsActive).ToList();

        var result = new List<PackageDetailDto>();

        foreach (var pkg in activePackages)
        {
            var pricing = await pricingRepository.GetByPackageIdAsync(pkg.Id, ct);
            var deliverables = await deliverableRepository.GetByPackageIdAsync(pkg.Id, ct);

            var deliverableDtos = new List<PackageDeliverableInfoDto>();
            foreach (var d in deliverables)
            {
                var master = await masterRepository.GetByIdAsync(d.DeliverableMasterId, ct);
                deliverableDtos.Add(new PackageDeliverableInfoDto(
                    d.Id,
                    master?.Name ?? "Unknown",
                    d.Quantity,
                    d.Configuration,
                    d.Notes
                ));
            }

            result.Add(new PackageDetailDto(
                pkg.Id,
                pkg.Name,
                pkg.Description,
                pkg.PackageType,
                pkg.CoverageType,
                pkg.IsActive,
                pricing is not null
                    ? new PackagePricingInfoDto(
                        pricing.PricingType, pricing.BasePrice, pricing.DiscountPercentage,
                        pricing.FinalPrice, pricing.DurationHours, pricing.IsNegotiable, pricing.Notes)
                    : null,
                deliverableDtos
            ));
        }

        return result;
    }
}
