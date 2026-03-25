using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.InitiateInstagramAuth;

public record InitiateInstagramAuthCommand(Guid ProfileId) : IRequest<InitiateInstagramAuthResponse>;

public record InitiateInstagramAuthResponse(string AuthorizationUrl);

public class InitiateInstagramAuthValidator : AbstractValidator<InitiateInstagramAuthCommand>
{
    public InitiateInstagramAuthValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class InitiateInstagramAuthHandler(
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IInstagramConnectionRepository connectionRepository,
    IInstagramApiClient instagramClient)
    : IRequestHandler<InitiateInstagramAuthCommand, InitiateInstagramAuthResponse>
{
    public async Task<InitiateInstagramAuthResponse> Handle(InitiateInstagramAuthCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (user.HasRole(ProfessionalRole.DigitalRental) && user.ProfessionalRoles.Count == 1)
            throw new DomainException("Digital Rental professionals use the Rental Store for their portfolio.");

        var existing = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null && existing.IsActive)
            throw new DomainException("Instagram is already connected. Disconnect first to reconnect.");

        var authUrl = instagramClient.GetAuthorizationUrl(request.ProfileId.ToString());
        return new InitiateInstagramAuthResponse(authUrl);
    }
}
