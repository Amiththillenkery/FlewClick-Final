using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.RegisterConsumer;

public record RegisterConsumerCommand(
    string Phone,
    string FullName,
    string Password,
    string? Email
) : IRequest<ConsumerAuthTokenResponse>;

public class RegisterConsumerValidator : AbstractValidator<RegisterConsumerCommand>
{
    public RegisterConsumerValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(254).When(x => x.Email is not null);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[^\w\s]").WithMessage("Password must contain at least one special character.");
    }
}

public class RegisterConsumerHandler(
    IConsumerRepository consumerRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<RegisterConsumerCommand, ConsumerAuthTokenResponse>
{
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 30;

    public async Task<ConsumerAuthTokenResponse> Handle(RegisterConsumerCommand request, CancellationToken ct)
    {
        var existing = await consumerRepository.GetByPhoneAsync(request.Phone, ct);
        if (existing is not null)
            throw new DomainException("An account with this phone number already exists.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await consumerRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), ct);
            if (emailExists is not null)
                throw new DomainException("An account with this email already exists.");
        }

        var consumer = Consumer.Create(request.Phone, request.FullName, request.Email);
        var hash = passwordHasher.Hash(request.Password);
        consumer.UpdatePassword(hash);
        consumer.VerifyPhone();
        consumer.MarkLoggedIn();
        await consumerRepository.AddAsync(consumer, ct);

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
