namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record RentalEquipmentDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string EquipmentName,
    string? EquipmentType,
    string? Brand,
    decimal DailyRentalRate,
    bool IsAvailable,
    string? ConditionNotes
);
