using Gamification.Core.Events;

namespace Gamification.Core.Models;

/// <summary>Результат одного начисления опыта (возвращает IXpCalculator).</summary>
/// <param name="Transaction">Созданная транзакция.</param>
/// <param name="OldLevel">Уровень до начисления.</param>
/// <param name="NewLevel">Уровень после начисления.</param>
/// <param name="TotalXp">Суммарный опыт после начисления.</param>
/// <param name="Events">События, порождённые начислением (XpGained, LevelUp).</param>
public sealed record XpGrantResult(
    XpTransaction Transaction, int OldLevel, int NewLevel, long TotalXp, IReadOnlyList<IGameEvent> Events)
{
    /// <summary>На сколько уровней вырос игрок.</summary>
    public int LevelUps => NewLevel - OldLevel;
}
