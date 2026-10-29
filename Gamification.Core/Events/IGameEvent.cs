namespace Gamification.Core.Events;

/// <summary>Игровое событие: факт, по которому проверяются правила достижений.</summary>
public interface IGameEvent
{
    /// <summary>Игрок, с которым произошло событие.</summary>
    Guid PlayerId { get; }

    /// <summary>Время события (UTC).</summary>
    DateTime OccurredAt { get; }
}
