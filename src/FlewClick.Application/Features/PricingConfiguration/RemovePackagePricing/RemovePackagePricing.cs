using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.PricingConfiguration.RemovePackagePricing;

public record RemovePackagePricingCommand(Guid PackageId) : IRequest;

public class RemovePackagePricingHandler(IPackagePricingRepository repository)
    : IRequestHandler<RemovePackagePricingCommand>
{
    public async Task Handle(RemovePackagePricingCommand request, CancellationToken ct)
    {
        var pricing = await repository.GetByPackageIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("PackagePricing for package", request.PackageId);

        await repository.RemoveAsync(pricing, ct);
    }
}
