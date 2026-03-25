using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlewClick.Infrastructure.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(Guid consumerId, string phone, string name)
    {
        var expirationMinutes = int.Parse(
            configuration["Jwt:ConsumerTokenExpirationMinutes"]
            ?? configuration["Jwt:ExpirationMinutes"]
            ?? "10080");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, consumerId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("phone", phone),
            new("name", name),
            new("user_type", "Consumer")
        };

        return BuildToken(claims, expirationMinutes);
    }

    public string GenerateProfessionalToken(
        Guid userId, Guid profileId, string email, string name, List<ProfessionalRole> roles)
    {
        var expirationMinutes = int.Parse(
            configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("profileId", profileId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("name", name),
            new("user_type", "Professional"),
            new("roles", JsonSerializer.Serialize(roles.Select(r => r.ToString())))
        };

        return BuildToken(claims, expirationMinutes);
    }

    private string BuildToken(List<Claim> claims, int expirationMinutes)
    {
        var secret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? "FlewClick";
        var audience = configuration["Jwt:Audience"] ?? "FlewClick";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
