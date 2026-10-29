using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>Игрок совершил не меньше count действий (с указанным источником или любым).</summary>
public sealed class ActionCountRule : IAchievementRule
{
    private readonly int _count;
    private readonly string? _source;

    /// <summary>Создаёт правило «count действий».</summary>
    /// <param name="count">Сколько действий нужно.</param>
    /// <param name="source">Источник действий; null — любой.</param>
    public ActionCountRule(int count, string? source = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        _count = count;
        _source = source;
    }

    /// <inheritdoc />
    public string RuleType => "ActionCount";

    /// <inheritdoc />
    public string Parameter => _source is null ? _count.ToString() : $"{_source}:{_count}";

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) =>
        player.CountActions(_source) >= _count;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player) =>
        Math.Min(1, player.CountActions(_source) / (double)_count);
}
