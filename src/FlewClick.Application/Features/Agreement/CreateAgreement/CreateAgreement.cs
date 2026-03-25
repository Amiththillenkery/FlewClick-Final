using FlewClick.Application.Features.Agreement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using DomainAgreement = FlewClick.Domain.Entities.Agreement;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Agreement.CreateAgreement;

public record CreateAgreementDeliverableItem(
    string DeliverableName,
    int Quantity,
    Dictionary<string, object?>? Configuration,
    string? Notes);

public record CreateAgreementCommand(
    Guid BookingRequestId,
    string PackageSnapshot,
    DateTime EventDate,
    string? EventLocation,
    string? EventDescription,
    decimal TotalPrice,
    string? Terms,
    string? Conditions,
    string? Notes,
    List<CreateAgreementDeliverableItem> Deliverables) : IRequest<AgreementDto>;

public class CreateAgreementValidator : AbstractValidator<CreateAgreementCommand>
{
    public CreateAgreementValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.PackageSnapshot).NotEmpty();
        RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Deliverables).NotNull();
        RuleForEach(x => x.Deliverables).ChildRules(d =>
        {
            d.RuleFor(i => i.DeliverableName).NotEmpty();
            d.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}

public class CreateAgreementHandler(
    IBookingRequestRepository bookingRepository,
    IAgreementRepository agreementRepository,
    IAgreementDeliverableRepository deliverableRepository,
    IBookingStatusHistoryRepository historyRepository,
    IConversationRepository conversationRepository)
    : IRequestHandler<CreateAgreementCommand, AgreementDto>
{
    public async Task<AgreementDto> Handle(CreateAgreementCommand request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingRequestId);

        if (booking.Status is not BookingStatus.PendingQuotation and not BookingStatus.RevisionRequested)
            throw new DomainException(
                $"Cannot create agreement when booking status is '{booking.Status}'. Expected '{BookingStatus.PendingQuotation}' or '{BookingStatus.RevisionRequested}'.");

        var previous = await agreementRepository.GetLatestByBookingIdAsync(request.BookingRequestId, ct);
        if (previous is not null)
        {
            previous.Supersede();
            await agreementRepository.UpdateAsync(previous, ct);
        }

        var version = await agreementRepository.GetNextVersionAsync(request.BookingRequestId, ct);
        var agreement = DomainAgreement.Create(
            request.BookingRequestId,
            version,
            request.PackageSnapshot,
            request.EventDate,
            request.TotalPrice,
            request.EventLocation,
            request.EventDescription,
            request.Terms,
            request.Conditions,
            request.Notes);
        agreement.Send();

        await agreementRepository.AddAsync(agreement, ct);

        var deliverableEntities = request.Deliverables
            .Select(d => AgreementDeliverable.Create(
                agreement.Id,
                d.DeliverableName,
                d.Quantity,
                d.Configuration,
                d.Notes))
            .ToList();

        if (deliverableEntities.Count > 0)
            await deliverableRepository.AddRangeAsync(deliverableEntities, ct);

        await EnsureConversationAsync(booking, ct);

        var fromStatus = booking.Status;
        booking.MarkQuotationSent();
        await bookingRepository.UpdateAsync(booking, ct);

        var history = BookingStatusHistory.Create(
            booking.Id,
            fromStatus,
            BookingStatus.QuotationSent,
            booking.ProfessionalProfileId.ToString(),
            MessageSenderType.Professional);
        await historyRepository.AddAsync(history, ct);

        var loadedDeliverables = await deliverableRepository.GetByAgreementIdAsync(agreement.Id, ct);
        var deliverableDtos = loadedDeliverables.ConvertAll(AgreementMapper.ToDto);

        return AgreementMapper.ToDto(agreement, deliverableDtos);
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
