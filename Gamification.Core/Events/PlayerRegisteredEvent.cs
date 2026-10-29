namespace Gamification.Core.Events;

/// <summary>Игрок зарегистрирован.</summary>
/// <param name="PlayerId">Новый игрок.</param>
public sealed record PlayerRegisteredEvent(Guid PlayerId) : IGameEvent
{
    /// <inheritdoc />
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
