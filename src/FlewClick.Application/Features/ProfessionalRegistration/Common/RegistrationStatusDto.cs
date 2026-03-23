using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record RegistrationStatusDto(
    Guid ProfileId,
    ProfessionalRole Role,
    bool ProfileComplete,
    bool ConfigComplete,
    bool IsRegistrationComplete
);
