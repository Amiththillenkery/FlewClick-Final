using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.CompleteBooking;

public record CompleteBookingCommand(Guid BookingId, Guid ProfessionalProfileId) : IRequest<BookingDto>;

public class CompleteBookingValidator : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class CompleteBookingHandler(
    IBookingRequestRepository bookingRepository,
    IBookingStatusHistoryRepository historyRepository,
    IAgreementRepository agreementRepository,
    IPlatformFeePaymentRepository platformFeePaymentRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<CompleteBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CompleteBookingCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingId);

        if (booking.ProfessionalProfileId != request.ProfessionalProfileId)
            throw new DomainException("This booking does not belong to your professional profile.");

        var profile = await profileRepository.GetByIdAsync(request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfessionalProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        var agreements = await agreementRepository.GetByBookingIdAsync(request.BookingId, ct);
        var acceptedAgreement = agreements
            .Where(a => a.Status == AgreementStatus.Accepted)
            .OrderByDescending(a => a.Version)
            .FirstOrDefault()
            ?? throw new DomainException("No accepted agreement was found for this booking.");

        var existingFee = await platformFeePaymentRepository.GetByBookingIdAsync(request.BookingId, ct);
        if (existingFee is not null)
            throw new DomainException("A platform fee payment already exists for this booking.");

        booking.Complete();
        await bookingRepository.UpdateAsync(booking, ct);

        var fee = PlatformFeePayment.Create(
            booking.Id,
            request.ProfessionalProfileId,
            acceptedAgreement.TotalPrice,
            feePercentage: 3.0m,
            gracePeriodDays: 15);

        await platformFeePaymentRepository.AddAsync(fee, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus: BookingStatus.Active,
            toStatus: BookingStatus.Completed,
            changedBy: user.FullName,
            changedByType: MessageSenderType.Professional);

        await historyRepository.AddAsync(history, ct);

        return BookingMapper.ToDto(booking);
    }
}
