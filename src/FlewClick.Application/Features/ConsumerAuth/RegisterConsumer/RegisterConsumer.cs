using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.RegisterConsumer;

public record RegisterConsumerCommand(string Phone, string FullName) : IRequest<RegisterConsumerResponse>;

public record RegisterConsumerResponse(string Message);

public class RegisterConsumerValidator : AbstractValidator<RegisterConsumerCommand>
{
    public RegisterConsumerValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
    }
}

public class RegisterConsumerHandler(
    IConsumerRepository consumerRepository,
    IOtpVerificationRepository otpRepository,
    ISmsService smsService)
    : IRequestHandler<RegisterConsumerCommand, RegisterConsumerResponse>
{
    public async Task<RegisterConsumerResponse> Handle(RegisterConsumerCommand request, CancellationToken ct)
    {
        var existing = await consumerRepository.GetByPhoneAsync(request.Phone, ct);
        if (existing is not null)
            throw new DomainException("An account with this phone number already exists.");

        var code = Random.Shared.Next(100000, 999999).ToString();
        var otp = OtpVerification.Create(request.Phone, code, OtpPurpose.Registration);

        await otpRepository.AddAsync(otp, ct);
        await smsService.SendOtpAsync(request.Phone, code, ct);

        return new RegisterConsumerResponse("OTP sent successfully. Verify to complete registration.");
    }
}
