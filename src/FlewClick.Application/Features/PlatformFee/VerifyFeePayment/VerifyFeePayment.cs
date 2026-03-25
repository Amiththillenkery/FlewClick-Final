using FlewClick.Application.Features.PlatformFee.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PlatformFee.VerifyFeePayment;

public record VerifyFeePaymentCommand(
    string RazorpayOrderId,
    string RazorpayPaymentId,
    string RazorpaySignature) : IRequest<PlatformFeeDto>;

public class VerifyFeePaymentValidator : AbstractValidator<VerifyFeePaymentCommand>
{
    public VerifyFeePaymentValidator()
    {
        RuleFor(x => x.RazorpayOrderId).NotEmpty();
        RuleFor(x => x.RazorpayPaymentId).NotEmpty();
        RuleFor(x => x.RazorpaySignature).NotEmpty();
    }
}

public class VerifyFeePaymentHandler(
    IPlatformFeePaymentRepository feeRepository,
    IRazorpayService razorpayService)
    : IRequestHandler<VerifyFeePaymentCommand, PlatformFeeDto>
{
    public async Task<PlatformFeeDto> Handle(VerifyFeePaymentCommand request, CancellationToken ct)
    {
        var isValid = razorpayService.VerifyPaymentSignature(
            request.RazorpayOrderId, request.RazorpayPaymentId, request.RazorpaySignature);

        var fee = await feeRepository.GetByRazorpayOrderIdAsync(request.RazorpayOrderId, ct)
            ?? throw new EntityNotFoundException("PlatformFeePayment", request.RazorpayOrderId);

        if (isValid)
        {
            if (fee.Status == PaymentStatus.Completed)
                return PlatformFeeMapper.ToDto(fee);

            if (fee.Status != PaymentStatus.Processing)
                throw new DomainException("Payment cannot be completed in the current state.");

            fee.MarkCompleted(request.RazorpayPaymentId, request.RazorpaySignature);
            await feeRepository.UpdateAsync(fee, ct);
        }
        else
        {
            if (fee.Status == PaymentStatus.Completed)
                throw new DomainException("Invalid payment signature.");

            if (fee.Status == PaymentStatus.Processing)
            {
                fee.MarkFailed("Payment signature verification failed.");
                await feeRepository.UpdateAsync(fee, ct);
            }
            else if (fee.Status != PaymentStatus.Failed)
                throw new DomainException("Payment signature verification failed.");
        }

        return PlatformFeeMapper.ToDto(fee);
    }
}
