using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>
/// Расчёт уровней по опыту. Формула: RequiredXp(n) = Base × n × (n − 1) / 2.
/// </summary>
public interface ILevelService
{
    /// <summary>Уровень, соответствующий суммарному опыту.</summary>
    /// <param name="xp">Суммарный опыт (не меньше 0).</param>
    /// <exception cref="ArgumentOutOfRangeException">xp отрицательный.</exception>
    int GetLevelForXp(long xp);

    /// <summary>Сколько суммарного опыта нужно для уровня.</summary>
    /// <param name="level">Уровень (не меньше 1).</param>
    /// <exception cref="ArgumentOutOfRangeException">level меньше 1.</exception>
    long GetXpForLevel(int level);

    /// <summary>Доля прохождения текущего уровня, от 0 до 1.</summary>
    /// <param name="xp">Суммарный опыт (не меньше 0).</param>
    double GetProgressToNextLevel(long xp);

    /// <summary>Полное описание прогресса: уровень, границы уровня, остаток до следующего.</summary>
    /// <param name="xp">Суммарный опыт (не меньше 0).</param>
    LevelProgress Describe(long xp);
}
