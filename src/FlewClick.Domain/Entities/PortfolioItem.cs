using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class PortfolioItem : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string InstagramMediaId { get; private set; } = string.Empty;
    public MediaType MediaType { get; private set; }
    public string MediaUrl { get; private set; } = string.Empty;
    public string? ThumbnailUrl { get; private set; }
    public string? Caption { get; private set; }
    public string Permalink { get; private set; } = string.Empty;
    public DateTime PostedAt { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsVisible { get; private set; }

    private PortfolioItem() { }

    public static PortfolioItem Create(
        Guid professionalProfileId,
        string instagramMediaId,
        MediaType mediaType,
        string mediaUrl,
        string? thumbnailUrl,
        string? caption,
        string permalink,
        DateTime postedAt,
        int displayOrder)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(instagramMediaId))
            throw new DomainException("Instagram media ID is required.");

        if (string.IsNullOrWhiteSpace(mediaUrl))
            throw new DomainException("Media URL is required.");

        if (string.IsNullOrWhiteSpace(permalink))
            throw new DomainException("Permalink is required.");

        return new PortfolioItem
        {
            ProfessionalProfileId = professionalProfileId,
            InstagramMediaId = instagramMediaId.Trim(),
            MediaType = mediaType,
            MediaUrl = mediaUrl.Trim(),
            ThumbnailUrl = thumbnailUrl?.Trim(),
            Caption = caption?.Trim(),
            Permalink = permalink.Trim(),
            PostedAt = postedAt,
            DisplayOrder = displayOrder,
            IsVisible = true
        };
    }

    public void UpdateMediaUrls(string mediaUrl, string? thumbnailUrl)
    {
        if (string.IsNullOrWhiteSpace(mediaUrl))
            throw new DomainException("Media URL is required.");

        MediaUrl = mediaUrl.Trim();
        ThumbnailUrl = thumbnailUrl?.Trim();
        Touch();
    }

    public void UpdateCaption(string? caption)
    {
        Caption = caption?.Trim();
        Touch();
    }

    public void SetVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        Touch();
    }

    public void SetDisplayOrder(int order)
    {
        if (order < 0) throw new DomainException("Display order cannot be negative.");
        DisplayOrder = order;
        Touch();
    }
}
