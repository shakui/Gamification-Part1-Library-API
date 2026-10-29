using Gamification.Core.Models;

namespace Gamification.Core.Events;

/// <summary>Игроку выдано достижение.</summary>
/// <param name="PlayerId">Игрок.</param>
/// <param name="Achievement">Выданное достижение.</param>
public sealed record AchievementUnlockedEvent(Guid PlayerId, AchievementDefinition Achievement) : IGameEvent
{
    /// <inheritdoc />
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
