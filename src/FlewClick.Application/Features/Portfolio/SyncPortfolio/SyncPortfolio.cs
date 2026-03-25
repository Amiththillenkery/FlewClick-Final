using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.SyncPortfolio;

public record SyncPortfolioCommand(Guid ProfileId) : IRequest<SyncPortfolioResponse>;

public record SyncPortfolioResponse(int TotalItems, int NewItems, int UpdatedItems);

public class SyncPortfolioValidator : AbstractValidator<SyncPortfolioCommand>
{
    public SyncPortfolioValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class SyncPortfolioHandler(
    IInstagramConnectionRepository connectionRepository,
    IPortfolioItemRepository portfolioItemRepository,
    IInstagramApiClient instagramClient)
    : IRequestHandler<SyncPortfolioCommand, SyncPortfolioResponse>
{
    public async Task<SyncPortfolioResponse> Handle(SyncPortfolioCommand request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("InstagramConnection for profile", request.ProfileId);

        if (!connection.IsActive)
            throw new DomainException("Instagram connection is not active.");

        if (connection.IsTokenExpired())
            throw new DomainException("Instagram token has expired. Please refresh or reconnect.");

        var media = await instagramClient.GetUserMediaAsync(
            connection.InstagramUserId, connection.AccessToken, 50, ct);

        var existingItems = await portfolioItemRepository.GetByProfileIdAsync(request.ProfileId, ct: ct);
        var existingByMediaId = existingItems.ToDictionary(i => i.InstagramMediaId);

        int newCount = 0, updatedCount = 0;
        var newItems = new List<PortfolioItem>();
        var updatedItems = new List<PortfolioItem>();

        for (int i = 0; i < media.Count; i++)
        {
            var m = media[i];
            var mediaType = ParseMediaType(m.MediaType);

            if (existingByMediaId.TryGetValue(m.Id, out var existing))
            {
                existing.UpdateMediaUrls(m.MediaUrl ?? m.Permalink, m.ThumbnailUrl);
                existing.UpdateCaption(m.Caption);
                updatedItems.Add(existing);
                updatedCount++;
            }
            else
            {
                var item = PortfolioItem.Create(
                    request.ProfileId,
                    m.Id,
                    mediaType,
                    m.MediaUrl ?? m.Permalink,
                    m.ThumbnailUrl,
                    m.Caption,
                    m.Permalink,
                    m.Timestamp,
                    i);

                newItems.Add(item);
                newCount++;
            }
        }

        if (newItems.Count > 0)
            await portfolioItemRepository.AddRangeAsync(newItems, ct);

        if (updatedItems.Count > 0)
            await portfolioItemRepository.UpdateRangeAsync(updatedItems, ct);

        connection.MarkSynced();
        await connectionRepository.UpdateAsync(connection, ct);

        return new SyncPortfolioResponse(media.Count, newCount, updatedCount);
    }

    private static MediaType ParseMediaType(string igMediaType) => igMediaType.ToUpperInvariant() switch
    {
        "IMAGE" => MediaType.Image,
        "VIDEO" => MediaType.Video,
        "CAROUSEL_ALBUM" => MediaType.CarouselAlbum,
        _ => MediaType.Image
    };
}
