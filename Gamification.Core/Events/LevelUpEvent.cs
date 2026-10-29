namespace Gamification.Core.Events;

/// <summary>Игрок повысил уровень (возможно, сразу на несколько).</summary>
/// <param name="PlayerId">Игрок.</param>
/// <param name="OldLevel">Уровень до повышения.</param>
/// <param name="NewLevel">Уровень после повышения.</param>
public sealed record LevelUpEvent(Guid PlayerId, int OldLevel, int NewLevel) : IGameEvent
{
    /// <inheritdoc />
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
