using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Shortlist.Common;

public record SavedProfessionalDto(
    Guid Id,
    Guid ConsumerId,
    Guid ProfessionalProfileId,
    string ProfessionalName,
    string? Location,
    decimal? HourlyRate,
    List<ProfessionalRole> ProfessionalRoles,
    DateTime SavedAt
);
