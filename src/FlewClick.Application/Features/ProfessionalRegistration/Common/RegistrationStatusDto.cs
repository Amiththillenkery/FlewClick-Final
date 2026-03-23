using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record RegistrationStatusDto(
    Guid ProfileId,
    List<ProfessionalRole> Roles,
    bool ProfileComplete,
    Dictionary<ProfessionalRole, bool> ConfigStatus,
    bool IsRegistrationComplete
);
