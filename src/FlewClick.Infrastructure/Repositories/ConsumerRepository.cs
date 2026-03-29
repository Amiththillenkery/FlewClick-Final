using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class ConsumerRepository(FlewClickDbContext context) : IConsumerRepository
{
    public async Task<Consumer?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Consumers.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Consumer?> GetByPhoneAsync(string phone, CancellationToken ct = default) =>
        await context.Consumers.FirstOrDefaultAsync(c => c.Phone == phone, ct);

    public async Task<Consumer?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await context.Consumers.FirstOrDefaultAsync(c => c.Email == email, ct);

    public async Task AddAsync(Consumer consumer, CancellationToken ct = default)
    {
        await context.Consumers.AddAsync(consumer, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Consumer consumer, CancellationToken ct = default)
    {
        context.Consumers.Update(consumer);
        await context.SaveChangesAsync(ct);
    }
}
