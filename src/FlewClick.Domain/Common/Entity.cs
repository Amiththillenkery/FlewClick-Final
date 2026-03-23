namespace FlewClick.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; protected init; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
