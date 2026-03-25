using FlewClick.Application.Features.ProfessionalAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalAuth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenRefreshResponse>;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IAppUserRepository userRepository,
    IProfessionalProfileRepository profileRepository,
    IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, TokenRefreshResponse>
{
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 30;

    public async Task<TokenRefreshResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var existingToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, ct)
            ?? throw new DomainException("Invalid refresh token.");

        if (existingToken.IsRevoked)
            throw new DomainException("Refresh token has been revoked.");

        if (existingToken.IsExpired)
            throw new DomainException("Refresh token has expired. Please login again.");

        var user = await userRepository.GetByIdAsync(existingToken.AppUserId, ct)
            ?? throw new DomainException("User not found.");

        if (user.UserType != UserType.ProfessionalUser)
            throw new DomainException("Invalid token.");

        var profile = await profileRepository.GetByAppUserIdAsync(user.Id, ct)
            ?? throw new DomainException("Professional profile not found.");

        var newRefreshToken = Domain.Entities.RefreshToken.Create(
            user.Id, DateTime.UtcNow.AddDays(RefreshTokenExpiryDays));

        existingToken.Revoke(newRefreshToken.Token);
        await refreshTokenRepository.UpdateAsync(existingToken, ct);
        await refreshTokenRepository.AddAsync(newRefreshToken, ct);

        var accessToken = jwtService.GenerateProfessionalToken(
            user.Id, profile.Id, user.Email, user.FullName, user.ProfessionalRoles);

        return new TokenRefreshResponse(
            accessToken,
            newRefreshToken.Token,
            AccessTokenExpiryMinutes * 60
        );
    }
}
