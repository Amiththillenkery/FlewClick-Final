using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface IOtpVerificationRepository
{
    Task<OtpVerification?> GetLatestByPhoneAndPurposeAsync(string phone, OtpPurpose purpose, CancellationToken ct = default);
    Task AddAsync(OtpVerification otp, CancellationToken ct = default);
    Task UpdateAsync(OtpVerification otp, CancellationToken ct = default);
}
