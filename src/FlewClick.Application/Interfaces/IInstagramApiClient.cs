namespace FlewClick.Application.Interfaces;

public record InstagramTokenResponse(string AccessToken, string UserId);
public record InstagramLongLivedTokenResponse(string AccessToken, DateTime ExpiresAt);
public record InstagramMediaItem(
    string Id,
    string MediaType,
    string? MediaUrl,
    string? ThumbnailUrl,
    string? Caption,
    string Permalink,
    DateTime Timestamp);

public interface IInstagramApiClient
{
    string GetAuthorizationUrl(string state);
    Task<InstagramTokenResponse> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default);
    Task<InstagramLongLivedTokenResponse> GetLongLivedTokenAsync(string shortLivedToken, CancellationToken ct = default);
    Task<InstagramLongLivedTokenResponse> RefreshLongLivedTokenAsync(string token, CancellationToken ct = default);
    Task<string> GetUsernameAsync(string userId, string accessToken, CancellationToken ct = default);
    Task<List<InstagramMediaItem>> GetUserMediaAsync(string userId, string accessToken, int limit = 50, CancellationToken ct = default);
}
