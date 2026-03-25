using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.ConsumerAuth.Common;

public static class ConsumerMapper
{
    public static ConsumerDto ToDto(Consumer consumer) =>
        new(
            consumer.Id,
            consumer.Phone,
            consumer.FullName,
            consumer.Email,
            consumer.IsPhoneVerified,
            consumer.IsActive,
            consumer.LastLoginAt,
            consumer.CreatedAtUtc
        );
}
