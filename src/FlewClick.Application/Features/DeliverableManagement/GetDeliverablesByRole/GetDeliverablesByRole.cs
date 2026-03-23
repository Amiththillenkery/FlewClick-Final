using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Application.Features.DeliverableManagement.GetDeliverablesByRole;

public record GetDeliverablesByRoleQuery(ProfessionalRole Role) : IRequest<IReadOnlyList<DeliverableMasterDto>>;

public class GetDeliverablesByRoleHandler(IDeliverableMasterRepository repository)
    : IRequestHandler<GetDeliverablesByRoleQuery, IReadOnlyList<DeliverableMasterDto>>
{
    public async Task<IReadOnlyList<DeliverableMasterDto>> Handle(GetDeliverablesByRoleQuery request, CancellationToken ct)
    {
        var masters = await repository.GetByRoleAsync(request.Role, ct);
        return masters.Select(DeliverableMapper.ToDto).ToList();
    }
}
