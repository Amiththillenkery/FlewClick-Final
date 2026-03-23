using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.ConfigureRentalEquipment;

public record AddRentalEquipmentCommand(
    Guid ProfileId,
    string EquipmentName,
    string? EquipmentType,
    string? Brand,
    decimal DailyRentalRate,
    string? ConditionNotes
) : IRequest<RentalEquipmentDto>;

public class AddRentalEquipmentValidator : AbstractValidator<AddRentalEquipmentCommand>
{
    public AddRentalEquipmentValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.EquipmentName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EquipmentType).MaximumLength(100).When(x => x.EquipmentType is not null);
        RuleFor(x => x.Brand).MaximumLength(100).When(x => x.Brand is not null);
        RuleFor(x => x.DailyRentalRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ConditionNotes).MaximumLength(500).When(x => x.ConditionNotes is not null);
    }
}

public class AddRentalEquipmentHandler(
    IProfessionalProfileRepository profileRepository,
    IRentalEquipmentRepository equipmentRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<AddRentalEquipmentCommand, RentalEquipmentDto>
{
    public async Task<RentalEquipmentDto> Handle(AddRentalEquipmentCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (user.ProfessionalRole != ProfessionalRole.DigitalRental)
            throw new DomainException("Rental equipment is only for Digital Rental professionals.");

        var equipment = RentalEquipment.Create(
            request.ProfileId, request.EquipmentName, request.EquipmentType,
            request.Brand, request.DailyRentalRate, request.ConditionNotes);
        await equipmentRepository.AddAsync(equipment, ct);
        return RegistrationMapper.ToDto(equipment);
    }
}
