using FlewClick.Application.Features.Agreement.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Agreement.GetAgreementHistory;

public record GetAgreementHistoryQuery(Guid BookingRequestId) : IRequest<List<AgreementDto>>;

public class GetAgreementHistoryValidator : AbstractValidator<GetAgreementHistoryQuery>
{
    public GetAgreementHistoryValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
    }
}

public class GetAgreementHistoryHandler(
    IAgreementRepository agreementRepository,
    IAgreementDeliverableRepository deliverableRepository)
    : IRequestHandler<GetAgreementHistoryQuery, List<AgreementDto>>
{
    public async Task<List<AgreementDto>> Handle(GetAgreementHistoryQuery request, CancellationToken ct)
    {
        var agreements = await agreementRepository.GetByBookingIdAsync(request.BookingRequestId, ct);
        var ordered = agreements.OrderByDescending(a => a.Version).ToList();

        var result = new List<AgreementDto>(ordered.Count);
        foreach (var agreement in ordered)
        {
            var deliverables = await deliverableRepository.GetByAgreementIdAsync(agreement.Id, ct);
            var deliverableDtos = deliverables.ConvertAll(AgreementMapper.ToDto);
            result.Add(AgreementMapper.ToDto(agreement, deliverableDtos));
        }

        return result;
    }
}
