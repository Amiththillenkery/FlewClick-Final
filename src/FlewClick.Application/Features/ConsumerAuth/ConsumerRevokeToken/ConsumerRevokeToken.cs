using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.ConsumerRevokeToken;

public record ConsumerRevokeTokenCommand(string RefreshToken) : IRequest;

public class ConsumerRevokeTokenValidator : AbstractValidator<ConsumerRevokeTokenCommand>
{
    public ConsumerRevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class ConsumerRevokeTokenHandler(
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<ConsumerRevokeTokenCommand>
{
    public async Task Handle(ConsumerRevokeTokenCommand request, CancellationToken ct)
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
