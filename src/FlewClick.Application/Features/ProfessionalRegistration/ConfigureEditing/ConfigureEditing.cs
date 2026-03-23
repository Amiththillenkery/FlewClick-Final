using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.ConfigureEditing;

public record ConfigureEditingCommand(
    Guid ProfileId,
    List<string> SoftwareTools,
    List<string> Specializations,
    string? OutputFormats
) : IRequest<EditingConfigDto>;

public class ConfigureEditingValidator : AbstractValidator<ConfigureEditingCommand>
{
    public ConfigureEditingValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.SoftwareTools).NotEmpty().WithMessage("At least one software tool is required.");
        RuleFor(x => x.OutputFormats).MaximumLength(500).When(x => x.OutputFormats is not null);
    }
}

public class ConfigureEditingHandler(
    IProfessionalProfileRepository profileRepository,
    IEditingConfigRepository configRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<ConfigureEditingCommand, EditingConfigDto>
{
    public async Task<EditingConfigDto> Handle(ConfigureEditingCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (user.ProfessionalRole != ProfessionalRole.Editor)
            throw new DomainException("Editing configuration is only for Editor professionals.");

        var existing = await configRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null)
        {
            existing.Update(request.SoftwareTools, request.Specializations, request.OutputFormats);
            await configRepository.UpdateAsync(existing, ct);
            return RegistrationMapper.ToDto(existing);
        }

        var config = EditingConfig.Create(
            request.ProfileId, request.SoftwareTools, request.Specializations, request.OutputFormats);
        await configRepository.AddAsync(config, ct);
        return RegistrationMapper.ToDto(config);
    }
}
