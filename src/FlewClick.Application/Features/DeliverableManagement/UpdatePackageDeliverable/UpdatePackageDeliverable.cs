using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.DeliverableManagement.UpdatePackageDeliverable;

public record UpdatePackageDeliverableCommand(
    Guid DeliverableId,
    int Quantity,
    Dictionary<string, object?>? Configuration,
    string? Notes
) : IRequest<PackageDeliverableDto>;

public class UpdatePackageDeliverableValidator : AbstractValidator<UpdatePackageDeliverableCommand>
{
    public UpdatePackageDeliverableValidator()
    {
        RuleFor(x => x.DeliverableId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes is not null);
    }
}

public class UpdatePackageDeliverableHandler(
    IPackageDeliverableRepository deliverableRepository,
    IDeliverableMasterRepository masterRepository)
    : IRequestHandler<UpdatePackageDeliverableCommand, PackageDeliverableDto>
{
    public async Task<PackageDeliverableDto> Handle(UpdatePackageDeliverableCommand request, CancellationToken ct)
    {
        var deliverable = await deliverableRepository.GetByIdAsync(request.DeliverableId, ct)
            ?? throw new EntityNotFoundException("PackageDeliverable", request.DeliverableId);

        var master = await masterRepository.GetByIdAsync(deliverable.DeliverableMasterId, ct)
            ?? throw new EntityNotFoundException("DeliverableMaster", deliverable.DeliverableMasterId);

        deliverable.Update(request.Quantity, request.Configuration, request.Notes);
        await deliverableRepository.UpdateAsync(deliverable, ct);
        return DeliverableMapper.ToDto(deliverable, master);
    }
}
