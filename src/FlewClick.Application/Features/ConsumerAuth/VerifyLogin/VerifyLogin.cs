using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.VerifyLogin;

public record VerifyLoginCommand(string Phone, string Otp) : IRequest<AuthResponseDto>;

public class VerifyLoginValidator : AbstractValidator<VerifyLoginCommand>
{
    public VerifyLoginValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
        RuleFor(x => x.Otp).NotEmpty().Length(6);
    }
}

public class VerifyLoginHandler(
    IConsumerRepository consumerRepository,
    IOtpVerificationRepository otpRepository,
    IJwtService jwtService)
    : IRequestHandler<VerifyLoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(VerifyLoginCommand request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByPhoneAsync(request.Phone, ct)
            ?? throw new DomainException("No account found with this phone number.");

        if (!consumer.IsActive)
            throw new DomainException("This account has been deactivated.");

        var otp = await otpRepository.GetLatestByPhoneAndPurposeAsync(request.Phone, OtpPurpose.Login, ct)
            ?? throw new DomainException("No OTP found. Request a new one.");

        if (otp.IsExpired())
            throw new DomainException("OTP has expired. Request a new one.");

        if (otp.IsUsed)
            throw new DomainException("OTP has already been used. Request a new one.");

        if (otp.Code != request.Otp)
            throw new DomainException("Invalid OTP.");

        otp.MarkUsed();
        await otpRepository.UpdateAsync(otp, ct);

        consumer.MarkLoggedIn();
        await consumerRepository.UpdateAsync(consumer, ct);

        var token = jwtService.GenerateToken(consumer.Id, consumer.Phone, consumer.FullName);
        return new AuthResponseDto(token, ConsumerMapper.ToDto(consumer));
    }
}
