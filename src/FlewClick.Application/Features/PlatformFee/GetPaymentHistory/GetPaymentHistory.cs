using FlewClick.Application.Features.PlatformFee.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PlatformFee.GetPaymentHistory;

public record GetPaymentHistoryQuery(Guid ProfessionalProfileId) : IRequest<List<PlatformFeeDto>>;

public class GetPaymentHistoryValidator : AbstractValidator<GetPaymentHistoryQuery>
{
    public GetPaymentHistoryValidator()
    {
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class GetPaymentHistoryHandler(IPlatformFeePaymentRepository feeRepository)
    : IRequestHandler<GetPaymentHistoryQuery, List<PlatformFeeDto>>
{
    public async Task<List<PlatformFeeDto>> Handle(GetPaymentHistoryQuery request, CancellationToken ct)
    {
        var fees = await feeRepository.GetByProfileIdAsync(request.ProfessionalProfileId, ct);
        return fees
            .OrderByDescending(f => f.CreatedAtUtc)
            .Select(PlatformFeeMapper.ToDto)
            .ToList();
    }
}
