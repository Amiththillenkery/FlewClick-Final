using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.CompleteInstagramAuth;

public record CompleteInstagramAuthCommand(string Code, Guid ProfileId) : IRequest<InstagramConnectionDto>;

public class CompleteInstagramAuthValidator : AbstractValidator<CompleteInstagramAuthCommand>
{
    public CompleteInstagramAuthValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class CompleteInstagramAuthHandler(
    IProfessionalProfileRepository profileRepository,
    IInstagramConnectionRepository connectionRepository,
    IInstagramApiClient instagramClient)
    : IRequestHandler<CompleteInstagramAuthCommand, InstagramConnectionDto>
{
    public async Task<InstagramConnectionDto> Handle(CompleteInstagramAuthCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var existing = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null && existing.IsActive)
            throw new DomainException("Instagram is already connected for this profile.");

        var tokenResponse = await instagramClient.ExchangeCodeForTokenAsync(request.Code, ct);

        var longLivedToken = await instagramClient.GetLongLivedTokenAsync(tokenResponse.AccessToken, ct);

        var username = await instagramClient.GetUsernameAsync(
            tokenResponse.UserId, longLivedToken.AccessToken, ct);

        if (existing is not null)
        {
            existing.UpdateToken(longLivedToken.AccessToken, longLivedToken.ExpiresAt);
            existing.Activate();
            await connectionRepository.UpdateAsync(existing, ct);
            return PortfolioMapper.ToDto(existing);
        }

        var connection = InstagramConnection.Create(
            request.ProfileId,
            tokenResponse.UserId,
            longLivedToken.AccessToken,
            longLivedToken.ExpiresAt,
            username);

        await connectionRepository.AddAsync(connection, ct);
        return PortfolioMapper.ToDto(connection);
    }
}
