namespace FlewClick.Application.Interfaces;

public interface ISmsService
{
    Task SendOtpAsync(string phone, string code, CancellationToken ct = default);
}
