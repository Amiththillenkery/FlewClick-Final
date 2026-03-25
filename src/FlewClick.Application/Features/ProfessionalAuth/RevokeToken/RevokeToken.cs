using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalAuth.RevokeToken;

public record RevokeTokenCommand(string RefreshToken) : IRequest;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RevokeTokenHandler(
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        var token = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, ct)
            ?? throw new DomainException("Invalid refresh token.");

        if (!token.IsRevoked)
        {
            token.Revoke();
            await refreshTokenRepository.UpdateAsync(token, ct);
        }
    }
}
