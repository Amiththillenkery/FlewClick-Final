using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.AcceptBookingRequest;

public record AcceptBookingRequestCommand(Guid BookingId, Guid ProfessionalProfileId) : IRequest<BookingDto>;

public class AcceptBookingRequestValidator : AbstractValidator<AcceptBookingRequestCommand>
{
    public AcceptBookingRequestValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class AcceptBookingRequestHandler(
    IPlatformFeePaymentRepository platformFeePaymentRepository,
    IBookingRequestRepository bookingRepository,
    IBookingStatusHistoryRepository historyRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IConversationRepository conversationRepository,
    INotificationService notificationService)
    : IRequestHandler<AcceptBookingRequestCommand, BookingDto>
{
    public async Task<BookingDto> Handle(AcceptBookingRequestCommand request, CancellationToken ct)
    {
        if (await platformFeePaymentRepository.HasOutstandingFeesAsync(request.ProfessionalProfileId, ct))
            throw new DomainException("Cannot accept booking requests while you have outstanding platform fees.");

        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingId);

        if (booking.ProfessionalProfileId != request.ProfessionalProfileId)
            throw new DomainException("This booking does not belong to your professional profile.");

        var profile = await profileRepository.GetByIdAsync(request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfessionalProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        booking.AcceptRequest();
        await bookingRepository.UpdateAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus: BookingStatus.Requested,
            toStatus: BookingStatus.PendingQuotation,
            changedBy: user.FullName,
            changedByType: MessageSenderType.Professional);

        await historyRepository.AddAsync(history, ct);

        await EnsureConversationAsync(booking, ct);

        await notificationService.NotifyBookingUpdatedAsync(
            booking.Id, 
            booking.ConsumerId, 
            booking.ProfessionalProfileId,
            booking.Status, 
            "Booking accepted by professional", 
            ct);

        return BookingMapper.ToDto(booking);
    }

    private async Task EnsureConversationAsync(BookingRequest booking, CancellationToken ct)
    {
        var existing = await conversationRepository.GetByBookingIdAsync(booking.Id, ct);
        if (existing is not null)
            return;

        var conversation = Conversation.Create(booking.Id, booking.ConsumerId, booking.ProfessionalProfileId);
        await conversationRepository.AddAsync(conversation, ct);
    }
}
