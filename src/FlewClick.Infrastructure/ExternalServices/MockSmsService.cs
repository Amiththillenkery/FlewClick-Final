using FlewClick.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlewClick.Infrastructure.ExternalServices;

public class MockSmsService(ILogger<MockSmsService> logger) : ISmsService
{
    public Task SendOtpAsync(string phone, string code, CancellationToken ct = default)
    {
        logger.LogInformation("=== MOCK SMS === OTP {Code} sent to {Phone}", code, phone);
        return Task.CompletedTask;
    }
}
