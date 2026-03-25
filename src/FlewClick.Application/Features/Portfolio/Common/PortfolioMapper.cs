using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.Portfolio.Common;

public static class PortfolioMapper
{
    public static PortfolioItemDto ToDto(PortfolioItem item) =>
        new(
            item.Id,
            item.ProfessionalProfileId,
            item.InstagramMediaId,
            item.MediaType,
            item.MediaUrl,
            item.ThumbnailUrl,
            item.Caption,
            item.Permalink,
            item.PostedAt,
            item.DisplayOrder,
            item.IsVisible,
            item.CreatedAtUtc,
            item.UpdatedAtUtc
        );

    public static InstagramConnectionDto ToDto(InstagramConnection connection) =>
        new(
            connection.Id,
            connection.ProfessionalProfileId,
            connection.InstagramUserId,
            connection.Username,
            connection.IsActive,
            connection.TokenExpiresAt,
            connection.IsTokenExpired(),
            connection.IsTokenExpiringSoon(),
            connection.LastSyncAt,
            connection.CreatedAtUtc
        );
}
