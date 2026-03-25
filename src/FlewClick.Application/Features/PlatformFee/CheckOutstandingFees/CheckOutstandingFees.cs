using FlewClick.Application.Features.PlatformFee.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PlatformFee.CheckOutstandingFees;

public record CheckOutstandingFeesQuery(Guid ProfessionalProfileId) : IRequest<OutstandingFeesDto>;

public class CheckOutstandingFeesValidator : AbstractValidator<CheckOutstandingFeesQuery>
{
    public CheckOutstandingFeesValidator()
    {
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class CheckOutstandingFeesHandler(IPlatformFeePaymentRepository feeRepository)
    : IRequestHandler<CheckOutstandingFeesQuery, OutstandingFeesDto>
{
    public async Task<OutstandingFeesDto> Handle(CheckOutstandingFeesQuery request, CancellationToken ct)
    {
        var fees = await feeRepository.GetOutstandingByProfileIdAsync(request.ProfessionalProfileId, ct);
        var dtos = fees.Select(PlatformFeeMapper.ToDto).ToList();
        var total = fees.Sum(f => f.FeeAmount);
        var isBlocked = fees.Count > 0;

        return new OutstandingFeesDto(isBlocked, total, dtos);
    }
}
