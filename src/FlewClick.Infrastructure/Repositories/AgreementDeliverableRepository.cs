using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class AgreementDeliverableRepository(FlewClickDbContext context) : IAgreementDeliverableRepository
{
    public async Task<List<AgreementDeliverable>> GetByAgreementIdAsync(Guid agreementId, CancellationToken ct = default) =>
        await context.AgreementDeliverables.Where(d => d.AgreementId == agreementId).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<AgreementDeliverable> deliverables, CancellationToken ct = default)
    {
        await context.AgreementDeliverables.AddRangeAsync(deliverables, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveByAgreementIdAsync(Guid agreementId, CancellationToken ct = default)
    {
        var items = await context.AgreementDeliverables.Where(d => d.AgreementId == agreementId).ToListAsync(ct);
        context.AgreementDeliverables.RemoveRange(items);
        await context.SaveChangesAsync(ct);
    }
}
