using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>Игрок набрал суммарно не меньше заданного опыта.</summary>
public sealed class TotalXpRule : IAchievementRule
{
    private readonly long _xp;

    /// <summary>Создаёт правило «набрано xp опыта».</summary>
    public TotalXpRule(long xp)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(xp);
        _xp = xp;
    }

    /// <inheritdoc />
    public string RuleType => "TotalXp";

    /// <inheritdoc />
    public string Parameter => _xp.ToString();

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) => player.TotalXp >= _xp;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player) => Math.Min(1, player.TotalXp / (double)_xp);
}
