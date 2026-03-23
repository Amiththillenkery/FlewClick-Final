using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.CompleteRegistration;

public record CompleteRegistrationCommand(Guid ProfileId) : IRequest;

public class CompleteRegistrationHandler(
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IPhotographyConfigRepository photographyRepo,
    IEditingConfigRepository editingRepo,
    IDroneConfigRepository droneRepo,
    IRentalEquipmentRepository rentalRepo)
    : IRequestHandler<CompleteRegistrationCommand>
{
    public async Task Handle(CompleteRegistrationCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        if (profile.IsRegistrationComplete)
            throw new DomainException("Registration is already complete.");

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        var missingConfigs = new List<string>();

        foreach (var role in user.ProfessionalRoles)
        {
            var configured = role switch
            {
                ProfessionalRole.Photographer or ProfessionalRole.Videographer =>
                    await photographyRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
                ProfessionalRole.Editor =>
                    await editingRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
                ProfessionalRole.DroneOwner =>
                    await droneRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
                ProfessionalRole.DigitalRental =>
                    (await rentalRepo.GetByProfileIdAsync(profile.Id, ct)).Count > 0,
                _ => throw new DomainException($"Unknown professional role: {role}.")
            };

            if (!configured)
                missingConfigs.Add(role.ToString());
        }

        if (missingConfigs.Count > 0)
            throw new DomainException($"Configuration for the following roles must be completed: {string.Join(", ", missingConfigs)}.");

        profile.MarkRegistrationComplete();
        await profileRepository.UpdateAsync(profile, ct);
    }
}
