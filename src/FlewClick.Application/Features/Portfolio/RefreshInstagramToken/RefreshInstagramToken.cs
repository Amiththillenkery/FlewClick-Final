using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.RefreshInstagramToken;

public record RefreshInstagramTokenCommand(Guid ProfileId) : IRequest<InstagramConnectionDto>;

public class RefreshInstagramTokenValidator : AbstractValidator<RefreshInstagramTokenCommand>
{
    public RefreshInstagramTokenValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class RefreshInstagramTokenHandler(
    IInstagramConnectionRepository connectionRepository,
    IInstagramApiClient instagramClient)
    : IRequestHandler<RefreshInstagramTokenCommand, InstagramConnectionDto>
{
    public async Task<InstagramConnectionDto> Handle(RefreshInstagramTokenCommand request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("InstagramConnection for profile", request.ProfileId);

        if (!connection.IsActive)
            throw new DomainException("Instagram connection is not active.");

        if (connection.IsTokenExpired())
            throw new DomainException("Token has already expired. Please reconnect Instagram.");

        var refreshed = await instagramClient.RefreshLongLivedTokenAsync(connection.AccessToken, ct);

        connection.UpdateToken(refreshed.AccessToken, refreshed.ExpiresAt);
        await connectionRepository.UpdateAsync(connection, ct);

        return PortfolioMapper.ToDto(connection);
    }
}
