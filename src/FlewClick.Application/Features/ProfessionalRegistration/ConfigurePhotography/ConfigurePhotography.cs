using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.ConfigurePhotography;

public record ConfigurePhotographyCommand(
    Guid ProfileId,
    List<string> Styles,
    string? CameraGear,
    string? ShootTypes,
    bool HasStudio
) : IRequest<PhotographyConfigDto>;

public class ConfigurePhotographyValidator : AbstractValidator<ConfigurePhotographyCommand>
{
    public ConfigurePhotographyValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Styles).NotEmpty().WithMessage("At least one photography style is required.");
        RuleFor(x => x.CameraGear).MaximumLength(500).When(x => x.CameraGear is not null);
        RuleFor(x => x.ShootTypes).MaximumLength(500).When(x => x.ShootTypes is not null);
    }
}

public class ConfigurePhotographyHandler(
    IProfessionalProfileRepository profileRepository,
    IPhotographyConfigRepository configRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<ConfigurePhotographyCommand, PhotographyConfigDto>
{
    public async Task<PhotographyConfigDto> Handle(ConfigurePhotographyCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (!user.HasAnyRole(ProfessionalRole.Photographer, ProfessionalRole.Videographer))
            throw new DomainException("Photography configuration is only for Photographer or Videographer professionals.");

        var existing = await configRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null)
        {
            existing.Update(request.Styles, request.CameraGear, request.ShootTypes, request.HasStudio);
            await configRepository.UpdateAsync(existing, ct);
            return RegistrationMapper.ToDto(existing);
        }

        var config = PhotographyConfig.Create(
            request.ProfileId, request.Styles, request.CameraGear, request.ShootTypes, request.HasStudio);
        await configRepository.AddAsync(config, ct);
        return RegistrationMapper.ToDto(config);
    }
}
