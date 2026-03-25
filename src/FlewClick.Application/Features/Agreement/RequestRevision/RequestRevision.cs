using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Agreement.RequestRevision;

public record RequestRevisionCommand(
    Guid BookingRequestId,
    Guid ConsumerId,
    string RevisionNotes) : IRequest;

/// <summary>Non-empty GUID used as <see cref="ChatMessage"/> sender for system messages.</summary>
internal static class AgreementSystemMessageSender
{
    public static readonly Guid Id = new("00000000-0000-0000-0000-000000000001");
}

public class RequestRevisionValidator : AbstractValidator<RequestRevisionCommand>
{
    public RequestRevisionValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.RevisionNotes).NotEmpty();
    }
}

public class RequestRevisionHandler(
    IBookingRequestRepository bookingRepository,
    IAgreementRepository agreementRepository,
    IBookingStatusHistoryRepository historyRepository,
    IConversationRepository conversationRepository,
    IChatMessageRepository chatMessageRepository)
    : IRequestHandler<RequestRevisionCommand>
{
    public async Task Handle(RequestRevisionCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingRequestId);

        if (booking.ConsumerId != request.ConsumerId)
            throw new DomainException("This booking does not belong to the current consumer.");

        if (booking.Status != BookingStatus.QuotationSent)
            throw new DomainException(
                $"Cannot request revision when booking status is '{booking.Status}'. Expected '{BookingStatus.QuotationSent}'.");

        var latest = await agreementRepository.GetLatestByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Agreement", request.BookingRequestId);

        latest.Reject();
        await agreementRepository.UpdateAsync(latest, ct);

        booking.RequestRevision();
        await bookingRepository.UpdateAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            BookingStatus.QuotationSent,
            BookingStatus.RevisionRequested,
            request.ConsumerId.ToString(),
            MessageSenderType.Consumer,
            request.RevisionNotes);
        await historyRepository.AddAsync(history, ct);

        var conversation = await conversationRepository.GetByBookingIdAsync(booking.Id, ct);
        if (conversation is null)
        {
            conversation = Conversation.Create(booking.Id, booking.ConsumerId, booking.ProfessionalProfileId);
            await conversationRepository.AddAsync(conversation, ct);
        }

        var message = ChatMessage.Create(
            conversation.Id,
            AgreementSystemMessageSender.Id,
            MessageSenderType.System,
            request.RevisionNotes);
        await chatMessageRepository.AddAsync(message, ct);
    }
}
