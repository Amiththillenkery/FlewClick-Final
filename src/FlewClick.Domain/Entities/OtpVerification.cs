using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class OtpVerification : Entity
{
    public string Phone { get; private init; } = string.Empty;
    public string Code { get; private init; } = string.Empty;
    public OtpPurpose Purpose { get; private init; }
    public DateTime ExpiresAt { get; private init; }
    public bool IsUsed { get; private set; }

    private OtpVerification() { }

    public static OtpVerification Create(string phone, string code, OtpPurpose purpose, int expiryMinutes = 5)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(code) || code.Length != 6)
            throw new DomainException("OTP code must be 6 digits.");

        return new OtpVerification
        {
            Phone = phone.Trim(),
            Code = code,
            Purpose = purpose,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            IsUsed = false
        };
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;

    public void MarkUsed()
    {
        if (IsUsed)
            throw new DomainException("OTP has already been used.");

        if (IsExpired())
            throw new DomainException("OTP has expired.");

        IsUsed = true;
        Touch();
    }
}
