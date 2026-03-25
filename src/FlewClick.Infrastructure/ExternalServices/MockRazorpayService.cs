using FlewClick.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlewClick.Infrastructure.ExternalServices;

public sealed class MockRazorpayService : IRazorpayService
{
    private readonly ILogger<MockRazorpayService> _logger;

    public MockRazorpayService(ILogger<MockRazorpayService> logger)
    {
        _logger = logger;
        _logger.LogWarning(
            "MockRazorpayService is active; replace with real Razorpay integration before production.");
    }

    public Task<RazorpayOrderResult> CreateOrderAsync(decimal amount, Guid bookingId, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var orderId = $"order_mock_{Guid.NewGuid():N}";
        var receipt = $"booking-{bookingId}";

        _logger.LogInformation(
            "Mock Razorpay: created order {OrderId} for amount {Amount} INR, receipt {Receipt}",
            orderId,
            amount,
            receipt);

        return Task.FromResult(new RazorpayOrderResult(orderId, amount, "INR", receipt));
    }

    public bool VerifyPaymentSignature(string orderId, string paymentId, string signature)
    {
        _logger.LogInformation(
            "Mock Razorpay: verifying payment signature for order {OrderId}, payment {PaymentId}, signature {Signature}",
            orderId,
            paymentId,
            signature);

        return true;
    }
}
