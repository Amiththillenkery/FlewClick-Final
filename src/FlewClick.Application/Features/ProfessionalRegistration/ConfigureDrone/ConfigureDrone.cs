using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.ConfigureDrone;

public record ConfigureDroneCommand(
    Guid ProfileId,
    string DroneModel,
    string? LicenseNumber,
    bool HasFlightCertification,
    int? MaxFlightAltitudeMeters,
    List<string> Capabilities
) : IRequest<DroneConfigDto>;

public class ConfigureDroneValidator : AbstractValidator<ConfigureDroneCommand>
{
    public ConfigureDroneValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.DroneModel).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LicenseNumber).MaximumLength(100).When(x => x.LicenseNumber is not null);
        RuleFor(x => x.MaxFlightAltitudeMeters).GreaterThan(0).When(x => x.MaxFlightAltitudeMeters.HasValue);
    }
}

public class ConfigureDroneHandler(
    IProfessionalProfileRepository profileRepository,
    IDroneConfigRepository configRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<ConfigureDroneCommand, DroneConfigDto>
{
    public async Task<DroneConfigDto> Handle(ConfigureDroneCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (!user.HasRole(ProfessionalRole.DroneOwner))
            throw new DomainException("Drone configuration is only for Drone Owner professionals.");

        var existing = await configRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null)
        {
            existing.Update(request.DroneModel, request.LicenseNumber,
                request.HasFlightCertification, request.MaxFlightAltitudeMeters, request.Capabilities);
            await configRepository.UpdateAsync(existing, ct);
            return RegistrationMapper.ToDto(existing);
        }

        var config = DroneConfig.Create(
            request.ProfileId, request.DroneModel, request.LicenseNumber,
            request.HasFlightCertification, request.MaxFlightAltitudeMeters, request.Capabilities);
        await configRepository.AddAsync(config, ct);
        return RegistrationMapper.ToDto(config);
    }
}
