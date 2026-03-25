using MediatR;

namespace FlewClick.Application.Features.Portfolio.HandleWebhookVerification;

public record HandleWebhookVerificationQuery(
    string Mode,
    string VerifyToken,
    string Challenge,
    string ConfiguredVerifyToken
) : IRequest<string?>;

public class HandleWebhookVerificationHandler
    : IRequestHandler<HandleWebhookVerificationQuery, string?>
{
    public Task<string?> Handle(HandleWebhookVerificationQuery request, CancellationToken ct)
    {
        if (request.Mode == "subscribe" && request.VerifyToken == request.ConfiguredVerifyToken)
            return Task.FromResult<string?>(request.Challenge);

        return Task.FromResult<string?>(null);
    }
}
