using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class DeliverableMasterRepository(FlewClickDbContext context) : IDeliverableMasterRepository
{
    public async Task<DeliverableMaster?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.DeliverableMasters.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IReadOnlyList<DeliverableMaster>> GetByRoleAsync(ProfessionalRole role, CancellationToken ct = default) =>
        await context.DeliverableMasters
            .Where(d => d.RoleType == role && d.IsActive)
            .OrderBy(d => d.Category).ThenBy(d => d.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<DeliverableMaster>> GetAllActiveAsync(CancellationToken ct = default) =>
        await context.DeliverableMasters
            .Where(d => d.IsActive)
            .OrderBy(d => d.RoleType).ThenBy(d => d.Name)
            .ToListAsync(ct);

    public async Task AddAsync(DeliverableMaster deliverable, CancellationToken ct = default)
    {
        await context.DeliverableMasters.AddAsync(deliverable, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DeliverableMaster deliverable, CancellationToken ct = default)
    {
        context.DeliverableMasters.Update(deliverable);
        await context.SaveChangesAsync(ct);
    }
}
