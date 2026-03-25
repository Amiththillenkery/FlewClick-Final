using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class InstagramConnection : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string InstagramUserId { get; private set; } = string.Empty;
    public string AccessToken { get; private set; } = string.Empty;
    public DateTime TokenExpiresAt { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime? LastSyncAt { get; private set; }

    private InstagramConnection() { }

    public static InstagramConnection Create(
        Guid professionalProfileId,
        string instagramUserId,
        string accessToken,
        DateTime tokenExpiresAt,
        string username)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(instagramUserId))
            throw new DomainException("Instagram user ID is required.");

        if (string.IsNullOrWhiteSpace(accessToken))
            throw new DomainException("Access token is required.");

        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Instagram username is required.");

        return new InstagramConnection
        {
            ProfessionalProfileId = professionalProfileId,
            InstagramUserId = instagramUserId.Trim(),
            AccessToken = accessToken,
            TokenExpiresAt = tokenExpiresAt,
            Username = username.Trim(),
            IsActive = true
        };
    }

    public void UpdateToken(string accessToken, DateTime tokenExpiresAt)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new DomainException("Access token is required.");

        AccessToken = accessToken;
        TokenExpiresAt = tokenExpiresAt;
        Touch();
    }

    public void MarkSynced()
    {
        LastSyncAt = DateTime.UtcNow;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public bool IsTokenExpired() => DateTime.UtcNow >= TokenExpiresAt;
    public bool IsTokenExpiringSoon(int daysThreshold = 7) =>
        DateTime.UtcNow >= TokenExpiresAt.AddDays(-daysThreshold);
}
