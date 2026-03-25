using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.DeclineBookingRequest;

public record DeclineBookingRequestCommand(Guid BookingId, Guid ProfessionalProfileId, string Reason)
    : IRequest<BookingDto>;

public class DeclineBookingRequestValidator : AbstractValidator<DeclineBookingRequestCommand>
{
    public DeclineBookingRequestValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(2000);
    }
}

public class DeclineBookingRequestHandler(
    IBookingRequestRepository bookingRepository,
    IBookingStatusHistoryRepository historyRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<DeclineBookingRequestCommand, BookingDto>
{
    public async Task<BookingDto> Handle(DeclineBookingRequestCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingId);

        if (booking.ProfessionalProfileId != request.ProfessionalProfileId)
            throw new DomainException("This booking does not belong to your professional profile.");

        var profile = await profileRepository.GetByIdAsync(request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfessionalProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        booking.Decline(request.Reason);
        await bookingRepository.UpdateAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus: BookingStatus.Requested,
            toStatus: BookingStatus.Declined,
            changedBy: user.FullName,
            changedByType: MessageSenderType.Professional,
            reason: request.Reason);

        await historyRepository.AddAsync(history, ct);

        return BookingMapper.ToDto(booking);
    }
}
