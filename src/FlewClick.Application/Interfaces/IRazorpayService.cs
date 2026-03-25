namespace FlewClick.Application.Interfaces;

public record RazorpayOrderResult(string OrderId, decimal Amount, string Currency, string Receipt);

public interface IRazorpayService
{
    Task<RazorpayOrderResult> CreateOrderAsync(decimal amount, Guid bookingId, CancellationToken ct = default);
    bool VerifyPaymentSignature(string orderId, string paymentId, string signature);
}
