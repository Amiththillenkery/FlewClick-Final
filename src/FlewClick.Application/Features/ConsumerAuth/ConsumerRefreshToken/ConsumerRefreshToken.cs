using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.ConsumerRefreshToken;

public record ConsumerRefreshTokenCommand(string RefreshToken) : IRequest<ConsumerTokenRefreshResponse>;

public class ConsumerRefreshTokenValidator : AbstractValidator<ConsumerRefreshTokenCommand>
{
    public ConsumerRefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class ConsumerRefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IConsumerRepository consumerRepository,
    IJwtService jwtService)
    : IRequestHandler<ConsumerRefreshTokenCommand, ConsumerTokenRefreshResponse>
{
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 30;

    public async Task<ConsumerTokenRefreshResponse> Handle(ConsumerRefreshTokenCommand request, CancellationToken ct)
    {
        var existingToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, ct)
            ?? throw new DomainException("Invalid refresh token.");

        if (existingToken.IsRevoked)
            throw new DomainException("Refresh token has been revoked.");

        if (existingToken.IsExpired)
            throw new DomainException("Refresh token has expired. Please login again.");

        var consumer = await consumerRepository.GetByIdAsync(existingToken.AppUserId, ct)
            ?? throw new DomainException("Consumer not found.");

        if (!consumer.IsActive)
            throw new DomainException("Your account has been deactivated.");

        var newRefreshToken = Domain.Entities.RefreshToken.Create(
            consumer.Id, DateTime.UtcNow.AddDays(RefreshTokenExpiryDays));

        existingToken.Revoke(newRefreshToken.Token);
        await refreshTokenRepository.UpdateAsync(existingToken, ct);
        await refreshTokenRepository.AddAsync(newRefreshToken, ct);

        var accessToken = jwtService.GenerateConsumerAccessToken(
            consumer.Id, consumer.Phone, consumer.FullName, consumer.Email);

        return new ConsumerTokenRefreshResponse(
            accessToken,
            newRefreshToken.Token,
            AccessTokenExpiryMinutes * 60
        );
    }
}
