using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class OtpVerificationRepository(FlewClickDbContext context) : IOtpVerificationRepository
{
    public async Task<OtpVerification?> GetLatestByPhoneAndPurposeAsync(
        string phone, OtpPurpose purpose, CancellationToken ct = default) =>
        await context.OtpVerifications
            .Where(o => o.Phone == phone && o.Purpose == purpose)
            .OrderByDescending(o => o.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);

    public async Task AddAsync(OtpVerification otp, CancellationToken ct = default)
    {
        await context.OtpVerifications.AddAsync(otp, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(OtpVerification otp, CancellationToken ct = default)
    {
        context.OtpVerifications.Update(otp);
        await context.SaveChangesAsync(ct);
    }
}
