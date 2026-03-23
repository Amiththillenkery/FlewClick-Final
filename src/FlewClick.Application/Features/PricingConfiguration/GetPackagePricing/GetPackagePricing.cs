using FlewClick.Application.Features.PricingConfiguration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.PricingConfiguration.GetPackagePricing;

public record GetPackagePricingQuery(Guid PackageId) : IRequest<PackagePricingDto?>;

public class GetPackagePricingHandler(IPackagePricingRepository repository)
    : IRequestHandler<GetPackagePricingQuery, PackagePricingDto?>
{
    public async Task<PackagePricingDto?> Handle(GetPackagePricingQuery request, CancellationToken ct)
    {
        var pricing = await repository.GetByPackageIdAsync(request.PackageId, ct);
        return pricing is not null ? PricingMapper.ToDto(pricing) : null;
    }
}
