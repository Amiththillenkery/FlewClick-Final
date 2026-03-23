using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.CreateProfessionalProfile;

public record CreateProfessionalProfileCommand(
    string FullName,
    string Email,
    string? Phone,
    List<ProfessionalRole> ProfessionalRoles,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl
) : IRequest<ProfessionalProfileDto>;

public class CreateProfessionalProfileValidator : AbstractValidator<CreateProfessionalProfileCommand>
{
    public CreateProfessionalProfileValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
        RuleFor(x => x.ProfessionalRoles).NotEmpty().WithMessage("At least one professional role is required.");
        RuleForEach(x => x.ProfessionalRoles).IsInEnum();
        RuleFor(x => x.Bio).MaximumLength(1000).When(x => x.Bio is not null);
        RuleFor(x => x.Location).MaximumLength(200).When(x => x.Location is not null);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).When(x => x.YearsOfExperience.HasValue);
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0).When(x => x.HourlyRate.HasValue);
        RuleFor(x => x.PortfolioUrl).MaximumLength(500).When(x => x.PortfolioUrl is not null);
    }
}

public class CreateProfessionalProfileHandler(
    IAppUserRepository userRepository,
    IProfessionalProfileRepository profileRepository)
    : IRequestHandler<CreateProfessionalProfileCommand, ProfessionalProfileDto>
{
    public async Task<ProfessionalProfileDto> Handle(CreateProfessionalProfileCommand request, CancellationToken ct)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser is not null)
            throw new DomainException($"A user with email '{request.Email}' already exists.");

        var user = AppUser.CreateProfessionalUser(
            request.FullName, request.Email, request.ProfessionalRoles, request.Phone);
        await userRepository.AddAsync(user, ct);

        var profile = ProfessionalProfile.Create(
            user.Id, request.Bio, request.Location,
            request.YearsOfExperience, request.HourlyRate, request.PortfolioUrl);
        await profileRepository.AddAsync(profile, ct);

        return RegistrationMapper.ToDto(profile);
    }
}
