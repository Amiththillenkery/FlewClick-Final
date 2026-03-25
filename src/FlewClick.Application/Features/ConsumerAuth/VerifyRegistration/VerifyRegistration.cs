using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.VerifyRegistration;

public record VerifyRegistrationCommand(string Phone, string Otp, string FullName) : IRequest<AuthResponseDto>;

public class VerifyRegistrationValidator : AbstractValidator<VerifyRegistrationCommand>
{
    public VerifyRegistrationValidator()
    {
        RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(15);
        RuleFor(x => x.Otp).NotEmpty().Length(6);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
    }
}

public class VerifyRegistrationHandler(
    IConsumerRepository consumerRepository,
    IOtpVerificationRepository otpRepository,
    IJwtService jwtService)
    : IRequestHandler<VerifyRegistrationCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(VerifyRegistrationCommand request, CancellationToken ct)
    {
        var otp = await otpRepository.GetLatestByPhoneAndPurposeAsync(request.Phone, OtpPurpose.Registration, ct)
            ?? throw new DomainException("No OTP found for this phone number. Request a new one.");

        if (otp.IsExpired())
            throw new DomainException("OTP has expired. Request a new one.");

        if (otp.IsUsed)
            throw new DomainException("OTP has already been used. Request a new one.");

        if (otp.Code != request.Otp)
            throw new DomainException("Invalid OTP.");

        var existing = await consumerRepository.GetByPhoneAsync(request.Phone, ct);
        if (existing is not null)
            throw new DomainException("An account with this phone number already exists.");

        otp.MarkUsed();
        await otpRepository.UpdateAsync(otp, ct);

        var consumer = Consumer.Create(request.Phone, request.FullName);
        consumer.VerifyPhone();
        consumer.MarkLoggedIn();
        await consumerRepository.AddAsync(consumer, ct);

        var token = jwtService.GenerateToken(consumer.Id, consumer.Phone, consumer.FullName);
        return new AuthResponseDto(token, ConsumerMapper.ToDto(consumer));
    }
}
