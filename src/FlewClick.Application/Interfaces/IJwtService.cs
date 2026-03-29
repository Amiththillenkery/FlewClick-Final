using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid consumerId, string phone, string name);
    string GenerateConsumerAccessToken(Guid consumerId, string phone, string name, string? email);
    string GenerateProfessionalToken(Guid userId, Guid profileId, string email, string name, List<ProfessionalRole> roles);
}
