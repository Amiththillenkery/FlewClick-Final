using FlewClick.Application.Features.Agreement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Agreement.AcceptAgreement;

public record AcceptAgreementCommand(Guid BookingRequestId, Guid ConsumerId) : IRequest<AgreementDto>;

public class AcceptAgreementValidator : AbstractValidator<AcceptAgreementCommand>
{
    public AcceptAgreementValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}

public class AcceptAgreementHandler(
    IBookingRequestRepository bookingRepository,
    IAgreementRepository agreementRepository,
    IAgreementDeliverableRepository deliverableRepository,
    IBookingStatusHistoryRepository historyRepository,
    INotificationService notificationService)
    : IRequestHandler<AcceptAgreementCommand, AgreementDto>
{
    public async Task<AgreementDto> Handle(AcceptAgreementCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingRequestId);

        if (booking.ConsumerId != request.ConsumerId)
            throw new DomainException("This booking does not belong to the current consumer.");

        if (booking.Status != BookingStatus.QuotationSent)
            throw new DomainException(
                $"Cannot accept agreement when booking status is '{booking.Status}'. Expected '{BookingStatus.QuotationSent}'.");

        var agreement = await agreementRepository.GetLatestByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Agreement", request.BookingRequestId);

        agreement.Accept();

        booking.AcceptAgreement();
        booking.Activate();

        await agreementRepository.UpdateAsync(agreement, ct);
        await bookingRepository.UpdateAsync(booking, ct);

        var acceptHistory = BookingStatusHistory.Create(
            booking.Id,
            BookingStatus.QuotationSent,
            BookingStatus.Accepted,
            request.ConsumerId.ToString(),
            MessageSenderType.Consumer);
        await historyRepository.AddAsync(acceptHistory, ct);

        var activateHistory = BookingStatusHistory.Create(
            booking.Id,
            BookingStatus.Accepted,
            BookingStatus.Active,
            "System",
            MessageSenderType.System);
        await historyRepository.AddAsync(activateHistory, ct);

        await notificationService.NotifyBookingUpdatedAsync(
            booking.Id, 
            booking.ConsumerId, 
            booking.ProfessionalProfileId, 
            booking.Status, 
            "Agreement accepted by consumer. Project is now active.", 
            ct);

        var deliverables = await deliverableRepository.GetByAgreementIdAsync(agreement.Id, ct);
        var deliverableDtos = deliverables.ConvertAll(AgreementMapper.ToDto);

        return AgreementMapper.ToDto(agreement, deliverableDtos);
    }
}
