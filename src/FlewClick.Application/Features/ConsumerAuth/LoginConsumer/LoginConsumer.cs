using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.LoginConsumer;

public record LoginConsumerCommand(string Phone) : IRequest<LoginConsumerResponse>;

public record LoginConsumerResponse(string Message);

public class LoginConsumerValidator : AbstractValidator<LoginConsumerCommand>
{
    public LoginConsumerValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
    }
}

public class LoginConsumerHandler(
    IConsumerRepository consumerRepository,
    IOtpVerificationRepository otpRepository,
    ISmsService smsService)
    : IRequestHandler<LoginConsumerCommand, LoginConsumerResponse>
{
    public async Task<LoginConsumerResponse> Handle(LoginConsumerCommand request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByPhoneAsync(request.Phone, ct)
            ?? throw new DomainException("No account found with this phone number. Please register first.");

        if (!consumer.IsActive)
            throw new DomainException("This account has been deactivated.");

        var code = Random.Shared.Next(100000, 999999).ToString();
        var otp = OtpVerification.Create(request.Phone, code, OtpPurpose.Login);

        await otpRepository.AddAsync(otp, ct);
        await smsService.SendOtpAsync(request.Phone, code, ct);

        return new LoginConsumerResponse("OTP sent successfully. Verify to login.");
    }
}
