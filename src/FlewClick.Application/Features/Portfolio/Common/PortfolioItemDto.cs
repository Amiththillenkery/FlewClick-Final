using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Portfolio.Common;

public record PortfolioItemDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string InstagramMediaId,
    MediaType MediaType,
    string MediaUrl,
    string? ThumbnailUrl,
    string? Caption,
    string Permalink,
    DateTime PostedAt,
    int DisplayOrder,
    bool IsVisible,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record InstagramConnectionDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string InstagramUserId,
    string Username,
    bool IsActive,
    DateTime TokenExpiresAt,
    bool IsTokenExpired,
    bool IsTokenExpiringSoon,
    DateTime? LastSyncAt,
    DateTime CreatedAtUtc
);
