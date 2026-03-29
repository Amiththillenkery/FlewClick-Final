using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.LoginConsumer;

public record LoginConsumerCommand(string Phone, string Password) : IRequest<ConsumerAuthTokenResponse>;

public class LoginConsumerValidator : AbstractValidator<LoginConsumerCommand>
{
    public LoginConsumerValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginConsumerHandler(
    IConsumerRepository consumerRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<LoginConsumerCommand, ConsumerAuthTokenResponse>
{
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 30;

    public async Task<ConsumerAuthTokenResponse> Handle(LoginConsumerCommand request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByPhoneAsync(request.Phone, ct)
            ?? throw new DomainException("Invalid phone number or password.");

        if (!consumer.IsActive)
            throw new DomainException("Your account has been deactivated. Contact support.");

        if (string.IsNullOrEmpty(consumer.PasswordHash))
            throw new DomainException("Password has not been set. Please register again.");

        if (!passwordHasher.Verify(request.Password, consumer.PasswordHash))
            throw new DomainException("Invalid phone number or password.");

        consumer.MarkLoggedIn();
        await consumerRepository.UpdateAsync(consumer, ct);

        var accessToken = jwtService.GenerateConsumerAccessToken(
            consumer.Id, consumer.Phone, consumer.FullName, consumer.Email);

        var refreshToken = RefreshToken.Create(
            consumer.Id, DateTime.UtcNow.AddDays(RefreshTokenExpiryDays));
        await refreshTokenRepository.AddAsync(refreshToken, ct);

        return new ConsumerAuthTokenResponse(
            accessToken,
            refreshToken.Token,
            AccessTokenExpiryMinutes * 60,
            ConsumerMapper.ToDto(consumer)
        );
    }
}
