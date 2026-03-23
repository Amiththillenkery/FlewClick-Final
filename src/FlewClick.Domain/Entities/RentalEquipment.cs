using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RentalEquipment : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string EquipmentName { get; private set; } = string.Empty;
    public string? EquipmentType { get; private set; }
    public string? Brand { get; private set; }
    public decimal DailyRentalRate { get; private set; }
    public bool IsAvailable { get; private set; }
    public string? ConditionNotes { get; private set; }

    private RentalEquipment() { }

    public static RentalEquipment Create(
        Guid professionalProfileId,
        string equipmentName,
        string? equipmentType,
        string? brand,
        decimal dailyRentalRate,
        string? conditionNotes)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(equipmentName))
            throw new DomainException("Equipment name is required.");

        if (dailyRentalRate < 0)
            throw new DomainException("Daily rental rate cannot be negative.");

        return new RentalEquipment
        {
            ProfessionalProfileId = professionalProfileId,
            EquipmentName = equipmentName.Trim(),
            EquipmentType = equipmentType?.Trim(),
            Brand = brand?.Trim(),
            DailyRentalRate = dailyRentalRate,
            IsAvailable = true,
            ConditionNotes = conditionNotes?.Trim()
        };
    }

    public void Update(
        string equipmentName,
        string? equipmentType,
        string? brand,
        decimal dailyRentalRate,
        string? conditionNotes)
    {
        if (string.IsNullOrWhiteSpace(equipmentName))
            throw new DomainException("Equipment name is required.");

        if (dailyRentalRate < 0)
            throw new DomainException("Daily rental rate cannot be negative.");

        EquipmentName = equipmentName.Trim();
        EquipmentType = equipmentType?.Trim();
        Brand = brand?.Trim();
        DailyRentalRate = dailyRentalRate;
        ConditionNotes = conditionNotes?.Trim();
        Touch();
    }

    public void MarkAvailable() { IsAvailable = true; Touch(); }
    public void MarkUnavailable() { IsAvailable = false; Touch(); }
}
