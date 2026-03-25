using FlewClick.Application.Features.Agreement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Agreement.GetAgreement;

public record GetAgreementQuery(Guid BookingRequestId) : IRequest<AgreementDto>;

public class GetAgreementValidator : AbstractValidator<GetAgreementQuery>
{
    public GetAgreementValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
    }
}

public class GetAgreementHandler(
    IAgreementRepository agreementRepository,
    IAgreementDeliverableRepository deliverableRepository)
    : IRequestHandler<GetAgreementQuery, AgreementDto>
{
    public async Task<AgreementDto> Handle(GetAgreementQuery request, CancellationToken ct)
    {
        var agreement = await agreementRepository.GetLatestByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Agreement", request.BookingRequestId);

        var deliverables = await deliverableRepository.GetByAgreementIdAsync(agreement.Id, ct);
        var deliverableDtos = deliverables.ConvertAll(AgreementMapper.ToDto);

        return AgreementMapper.ToDto(agreement, deliverableDtos);
    }
}
