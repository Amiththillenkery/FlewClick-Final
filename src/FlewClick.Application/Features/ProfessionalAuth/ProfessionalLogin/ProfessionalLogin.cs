using FlewClick.Application.Features.ProfessionalAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalAuth.ProfessionalLogin;

public record ProfessionalLoginCommand(string Email, string Password) : IRequest<AuthTokenResponse>;

public class ProfessionalLoginValidator : AbstractValidator<ProfessionalLoginCommand>
{
    public ProfessionalLoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class ProfessionalLoginHandler(
    IAppUserRepository userRepository,
    IProfessionalProfileRepository profileRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<ProfessionalLoginCommand, AuthTokenResponse>
{
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 30;

    public async Task<AuthTokenResponse> Handle(ProfessionalLoginCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), ct)
            ?? throw new DomainException("Invalid email or password.");

        if (user.UserType != UserType.ProfessionalUser)
            throw new DomainException("Invalid email or password.");

        if (!user.IsActive)
            throw new DomainException("Your account has been deactivated. Contact support.");

        if (string.IsNullOrEmpty(user.PasswordHash))
            throw new DomainException("Password has not been set. Please complete registration first.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Invalid email or password.");

        var profile = await profileRepository.GetByAppUserIdAsync(user.Id, ct)
            ?? throw new DomainException("Professional profile not found.");

        var accessToken = jwtService.GenerateProfessionalToken(
            user.Id, profile.Id, user.Email, user.FullName, user.ProfessionalRoles);

        var refreshToken = Domain.Entities.RefreshToken.Create(
            user.Id, DateTime.UtcNow.AddDays(RefreshTokenExpiryDays));
        await refreshTokenRepository.AddAsync(refreshToken, ct);

        return new AuthTokenResponse(
            accessToken,
            refreshToken.Token,
            AccessTokenExpiryMinutes * 60,
            new ProfessionalUserDto(
                user.Id,
                profile.Id,
                user.FullName,
                user.Email,
                user.Phone,
                user.ProfessionalRoles.Select(r => r.ToString()).ToList(),
                profile.Bio,
                profile.Location,
                profile.YearsOfExperience,
                profile.IsRegistrationComplete
            )
        );
    }
}
