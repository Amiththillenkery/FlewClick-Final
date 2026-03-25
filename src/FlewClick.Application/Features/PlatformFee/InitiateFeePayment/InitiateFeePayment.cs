using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PlatformFee.InitiateFeePayment;

public record InitiateFeePaymentCommand(Guid PlatformFeePaymentId, Guid ProfessionalProfileId)
    : IRequest<InitiateFeePaymentResponse>;

public record InitiateFeePaymentResponse(string OrderId, decimal Amount, string Currency);

public class InitiateFeePaymentValidator : AbstractValidator<InitiateFeePaymentCommand>
{
    public InitiateFeePaymentValidator()
    {
        RuleFor(x => x.PlatformFeePaymentId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class InitiateFeePaymentHandler(
    IPlatformFeePaymentRepository feeRepository,
    IRazorpayService razorpayService)
    : IRequestHandler<InitiateFeePaymentCommand, InitiateFeePaymentResponse>
{
    public async Task<InitiateFeePaymentResponse> Handle(InitiateFeePaymentCommand request, CancellationToken ct)
    {
        var fee = await feeRepository.GetByIdAsync(request.PlatformFeePaymentId, ct)
            ?? throw new EntityNotFoundException("PlatformFeePayment", request.PlatformFeePaymentId);

        if (fee.ProfessionalProfileId != request.ProfessionalProfileId)
            throw new DomainException("This platform fee does not belong to the specified professional.");

        if (fee.Status != PaymentStatus.Pending)
            throw new DomainException($"Cannot initiate payment when status is '{fee.Status}'.");

        var order = await razorpayService.CreateOrderAsync(fee.FeeAmount, fee.BookingRequestId, ct);
        fee.SetRazorpayOrderId(order.OrderId);
        await feeRepository.UpdateAsync(fee, ct);

        return new InitiateFeePaymentResponse(order.OrderId, order.Amount, order.Currency);
    }
}
