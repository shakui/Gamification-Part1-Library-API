namespace Gamification.Core.Events;

/// <summary>Игроку начислен опыт.</summary>
/// <param name="PlayerId">Игрок.</param>
/// <param name="Amount">Сколько опыта начислено.</param>
/// <param name="Source">Источник опыта.</param>
/// <param name="TotalXp">Суммарный опыт после начисления.</param>
public sealed record XpGainedEvent(Guid PlayerId, long Amount, string Source, long TotalXp) : IGameEvent
{
    /// <inheritdoc />
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
