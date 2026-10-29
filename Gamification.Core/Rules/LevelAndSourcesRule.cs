using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>Составное правило: достигнут уровень и использовано не меньше N разных источников.</summary>
public sealed class LevelAndSourcesRule : IAchievementRule
{
    private readonly int _level;
    private readonly int _sources;

    /// <summary>Создаёт правило «уровень level и не меньше sources источников».</summary>
    public LevelAndSourcesRule(int level, int sources)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 2);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sources);
        _level = level;
        _sources = sources;
    }

    /// <inheritdoc />
    public string RuleType => "LevelAndSources";

    /// <inheritdoc />
    public string Parameter => $"level:{_level};sources:{_sources}";

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) =>
        player.Level >= _level && player.CountDistinctSources() >= _sources;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player)
    {
        var levelPart = Math.Min(1, (player.Level - 1) / (double)(_level - 1));
        var sourcesPart = Math.Min(1, player.CountDistinctSources() / (double)_sources);
        return (levelPart + sourcesPart) / 2;
    }
}
