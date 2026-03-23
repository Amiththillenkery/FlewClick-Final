using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.DeliverableManagement.RemoveDeliverableFromPackage;

public record RemoveDeliverableFromPackageCommand(Guid DeliverableId) : IRequest;

public class RemoveDeliverableFromPackageHandler(IPackageDeliverableRepository repository)
    : IRequestHandler<RemoveDeliverableFromPackageCommand>
{
    public async Task Handle(RemoveDeliverableFromPackageCommand request, CancellationToken ct)
    {
        var deliverable = await repository.GetByIdAsync(request.DeliverableId, ct)
            ?? throw new EntityNotFoundException("PackageDeliverable", request.DeliverableId);

        await repository.RemoveAsync(deliverable, ct);
    }
}
