using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>
/// За одно действие начислено не меньше заданного опыта.
/// Пример правила, которому нужно само событие, а не только состояние игрока.
/// </summary>
public sealed class SingleGrantRule : IAchievementRule
{
    private readonly long _minAmount;

    /// <summary>Создаёт правило «одно начисление не меньше minAmount».</summary>
    public SingleGrantRule(long minAmount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minAmount);
        _minAmount = minAmount;
    }

    /// <inheritdoc />
    public string RuleType => "SingleGrant";

    /// <inheritdoc />
    public string Parameter => _minAmount.ToString();

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) =>
        gameEvent is XpGainedEvent gained
        && gained.Source != XpSources.AchievementReward
        && gained.Amount >= _minAmount;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player)
    {
        var best = player.Transactions.Where(t => !t.IsReward).Select(t => t.Amount).DefaultIfEmpty(0).Max();
        return Math.Min(1, best / (double)_minAmount);
    }
}
