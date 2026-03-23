using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.DeliverableManagement.GetPackageDeliverables;

public record GetPackageDeliverablesQuery(Guid PackageId) : IRequest<IReadOnlyList<PackageDeliverableDto>>;

public class GetPackageDeliverablesHandler(
    IPackageDeliverableRepository deliverableRepository,
    IDeliverableMasterRepository masterRepository)
    : IRequestHandler<GetPackageDeliverablesQuery, IReadOnlyList<PackageDeliverableDto>>
{
    public async Task<IReadOnlyList<PackageDeliverableDto>> Handle(GetPackageDeliverablesQuery request, CancellationToken ct)
    {
        var deliverables = await deliverableRepository.GetByPackageIdAsync(request.PackageId, ct);
        var result = new List<PackageDeliverableDto>();

        foreach (var d in deliverables)
        {
            var master = await masterRepository.GetByIdAsync(d.DeliverableMasterId, ct);
            if (master is not null)
                result.Add(DeliverableMapper.ToDto(d, master));
        }

        return result;
    }
}
