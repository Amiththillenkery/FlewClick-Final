using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlewClick.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FlewClick.Infrastructure.ExternalServices;

public class InstagramApiClient(HttpClient httpClient, IConfiguration configuration) : IInstagramApiClient
{
    private string AppId => configuration["Instagram:AppId"]
        ?? throw new InvalidOperationException("Instagram:AppId is not configured.");
    private string AppSecret => configuration["Instagram:AppSecret"]
        ?? throw new InvalidOperationException("Instagram:AppSecret is not configured.");
    private string RedirectUri => configuration["Instagram:RedirectUri"]
        ?? throw new InvalidOperationException("Instagram:RedirectUri is not configured.");

    public string GetAuthorizationUrl(string state)
    {
        return $"https://www.instagram.com/oauth/authorize" +
               $"?client_id={AppId}" +
               $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
               $"&scope=instagram_business_basic" +
               $"&response_type=code" +
               $"&state={Uri.EscapeDataString(state)}";
    }

    public async Task<InstagramTokenResponse> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = AppId,
            ["client_secret"] = AppSecret,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = RedirectUri,
            ["code"] = code
        });

        var response = await httpClient.PostAsync("https://api.instagram.com/oauth/access_token", content, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ShortLivedTokenResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse Instagram token response.");

        return new InstagramTokenResponse(result.AccessToken, result.UserId.ToString());
    }

    public async Task<InstagramLongLivedTokenResponse> GetLongLivedTokenAsync(string shortLivedToken, CancellationToken ct = default)
    {
        var url = $"https://graph.instagram.com/access_token" +
                  $"?grant_type=ig_exchange_token" +
                  $"&client_secret={AppSecret}" +
                  $"&access_token={shortLivedToken}";

        var response = await httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LongLivedTokenResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse long-lived token response.");

        var expiresAt = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
        return new InstagramLongLivedTokenResponse(result.AccessToken, expiresAt);
    }

    public async Task<InstagramLongLivedTokenResponse> RefreshLongLivedTokenAsync(string token, CancellationToken ct = default)
    {
        var url = $"https://graph.instagram.com/refresh_access_token" +
                  $"?grant_type=ig_refresh_token" +
                  $"&access_token={token}";

        var response = await httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LongLivedTokenResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse refreshed token response.");

        var expiresAt = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
        return new InstagramLongLivedTokenResponse(result.AccessToken, expiresAt);
    }

    public async Task<string> GetUsernameAsync(string userId, string accessToken, CancellationToken ct = default)
    {
        var url = $"https://graph.instagram.com/v21.0/{userId}?fields=username&access_token={accessToken}";

        var response = await httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserProfileResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse Instagram user profile response.");

        return result.Username;
    }

    public async Task<List<InstagramMediaItem>> GetUserMediaAsync(
        string userId, string accessToken, int limit = 50, CancellationToken ct = default)
    {
        var url = $"https://graph.instagram.com/v21.0/{userId}/media" +
                  $"?fields=id,caption,media_type,media_url,thumbnail_url,permalink,timestamp" +
                  $"&limit={limit}" +
                  $"&access_token={accessToken}";

        var response = await httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<MediaListResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse Instagram media response.");

        return result.Data.Select(m => new InstagramMediaItem(
            m.Id,
            m.MediaType,
            m.MediaUrl,
            m.ThumbnailUrl,
            m.Caption,
            m.Permalink,
            m.Timestamp
        )).ToList();
    }

    private record ShortLivedTokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("user_id")] long UserId);

    private record LongLivedTokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] long ExpiresIn);

    private record UserProfileResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("username")] string Username);

    private record MediaDataItem(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("media_type")] string MediaType,
        [property: JsonPropertyName("media_url")] string? MediaUrl,
        [property: JsonPropertyName("thumbnail_url")] string? ThumbnailUrl,
        [property: JsonPropertyName("caption")] string? Caption,
        [property: JsonPropertyName("permalink")] string Permalink,
        [property: JsonPropertyName("timestamp")] DateTime Timestamp);

    private record MediaListResponse(
        [property: JsonPropertyName("data")] List<MediaDataItem> Data);
}
