using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.CreateBookingRequest;

public record CreateBookingRequestCommand(
    Guid ConsumerId,
    Guid ProfessionalProfileId,
    Guid PackageId,
    DateTime EventDate,
    string? EventLocation,
    string? Notes
) : IRequest<BookingDto>;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequestCommand>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
        RuleFor(x => x.PackageId).NotEmpty();
        RuleFor(x => x.EventDate).Must(d => d > DateTime.UtcNow)
            .WithMessage("Event date must be in the future.");
    }
}

public class CreateBookingRequestHandler(
    IConsumerRepository consumerRepository,
    IProfessionalProfileRepository profileRepository,
    IPackageRepository packageRepository,
    IBookingRequestRepository bookingRepository,
    IBookingStatusHistoryRepository historyRepository,
    INotificationService notificationService)
    : IRequestHandler<CreateBookingRequestCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CreateBookingRequestCommand request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByIdAsync(request.ConsumerId, ct)
            ?? throw new EntityNotFoundException("Consumer", request.ConsumerId);

        var profile = await profileRepository.GetByIdAsync(request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfessionalProfileId);

        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        if (package.ProfessionalProfileId != request.ProfessionalProfileId)
            throw new DomainException("The selected package does not belong to this professional profile.");

        if (!package.IsActive)
            throw new DomainException("The selected package is not active.");

        var booking = BookingRequest.Create(
            request.ConsumerId,
            request.ProfessionalProfileId,
            request.PackageId,
            request.EventDate,
            request.EventLocation,
            request.Notes);

        await bookingRepository.AddAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus: null,
            toStatus: BookingStatus.Requested,
            changedBy: consumer.FullName,
            changedByType: MessageSenderType.Consumer);

        await historyRepository.AddAsync(history, ct);

        await notificationService.NotifyNewBookingRequestAsync(
            booking.ProfessionalProfileId, 
            booking.Id, 
            consumer.FullName, 
            ct);

        return BookingMapper.ToDto(booking);
    }
}
