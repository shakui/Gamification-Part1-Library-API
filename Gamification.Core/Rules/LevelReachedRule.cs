using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>Игрок достиг уровня не ниже заданного.</summary>
public sealed class LevelReachedRule : IAchievementRule
{
    private readonly int _level;

    /// <summary>Создаёт правило «достигнут уровень level».</summary>
    public LevelReachedRule(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 2);
        _level = level;
    }

    /// <inheritdoc />
    public string RuleType => "LevelReached";

    /// <inheritdoc />
    public string Parameter => _level.ToString();

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) => player.Level >= _level;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player) =>
        Math.Min(1, (player.Level - 1) / (double)(_level - 1));
}
