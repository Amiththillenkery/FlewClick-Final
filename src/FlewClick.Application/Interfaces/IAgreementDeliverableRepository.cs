using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IAgreementDeliverableRepository
{
    Task<List<AgreementDeliverable>> GetByAgreementIdAsync(Guid agreementId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<AgreementDeliverable> deliverables, CancellationToken ct = default);
    Task RemoveByAgreementIdAsync(Guid agreementId, CancellationToken ct = default);
}
