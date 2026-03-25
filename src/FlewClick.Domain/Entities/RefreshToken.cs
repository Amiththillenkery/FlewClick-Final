using System.Security.Cryptography;
using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RefreshToken : Entity
{
    public Guid AppUserId { get; private init; }
    public string Token { get; private init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private init; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public string? CreatedByIp { get; private init; }
    public string? ReplacedByToken { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public static RefreshToken Create(Guid appUserId, DateTime expiresAtUtc, string? createdByIp = null)
    {
        if (appUserId == Guid.Empty) throw new DomainException("App user ID is required.");
        if (expiresAtUtc <= DateTime.UtcNow) throw new DomainException("Expiry must be in the future.");

        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            AppUserId = appUserId,
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAtUtc = expiresAtUtc,
            IsRevoked = false,
            CreatedByIp = createdByIp
        };
    }

    public void Revoke(string? replacedByToken = null)
    {
        if (IsRevoked) throw new DomainException("Token is already revoked.");
        IsRevoked = true;
        RevokedAtUtc = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
        Touch();
    }
}
