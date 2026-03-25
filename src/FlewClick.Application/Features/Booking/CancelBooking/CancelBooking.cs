using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.CancelBooking;

public record CancelBookingCommand(
    Guid BookingId,
    Guid CancelledById,
    MessageSenderType CancelledByType,
    string Reason
) : IRequest<BookingDto>;

public class CancelBookingValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CancelledById).NotEmpty().When(x => x.CancelledByType != MessageSenderType.System);
        RuleFor(x => x.CancelledByType).IsInEnum();
    }
}

public class CancelBookingHandler(
    IBookingRequestRepository bookingRepository,
    IBookingStatusHistoryRepository historyRepository,
    IConsumerRepository consumerRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<CancelBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CancelBookingCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingId);

        switch (request.CancelledByType)
        {
            case MessageSenderType.Consumer:
                if (request.CancelledById != booking.ConsumerId)
                    throw new DomainException("CancelledById does not match the booking consumer.");
                break;
            case MessageSenderType.Professional:
                if (request.CancelledById != booking.ProfessionalProfileId)
                    throw new DomainException("CancelledById does not match the booking professional profile.");
                break;
            case MessageSenderType.System:
                break;
            default:
                throw new DomainException("Unsupported cancellation sender type.");
        }

        var fromStatus = booking.Status;

        var changedBy = await ResolveChangedByAsync(request, ct);

        booking.Cancel(request.Reason, request.CancelledByType);
        await bookingRepository.UpdateAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus: fromStatus,
            toStatus: BookingStatus.Cancelled,
            changedBy: changedBy,
            changedByType: request.CancelledByType,
            reason: request.Reason);

        await historyRepository.AddAsync(history, ct);

        return BookingMapper.ToDto(booking);
    }

    private async Task<string> ResolveChangedByAsync(CancelBookingCommand request, CancellationToken ct)
    {
        switch (request.CancelledByType)
        {
            case MessageSenderType.Consumer:
                var consumer = await consumerRepository.GetByIdAsync(request.CancelledById, ct)
                    ?? throw new EntityNotFoundException("Consumer", request.CancelledById);
                return consumer.FullName;
            case MessageSenderType.Professional:
                var profile = await profileRepository.GetByIdAsync(request.CancelledById, ct)
                    ?? throw new EntityNotFoundException("ProfessionalProfile", request.CancelledById);
                var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
                    ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);
                return user.FullName;
            case MessageSenderType.System:
                return "System";
            default:
                throw new DomainException("Unsupported cancellation sender type.");
        }
    }
}
