using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>
/// Квадратичная прогрессия уровней: RequiredXp(n) = Base × n × (n − 1) / 2.
/// Уровень 2 — 100 XP, 3 — 300 XP, 4 — 600 XP, 5 — 1000 XP (при Base = 100).
/// Класс без состояния: результат зависит только от аргументов.
/// </summary>
public sealed class LevelService : ILevelService
{
    /// <summary>Базовый шаг по умолчанию.</summary>
    public const int DefaultBaseXp = 100;

    private readonly int _baseXp;

    /// <summary>Создаёт сервис с базовым шагом baseXp.</summary>
    public LevelService(int baseXp = DefaultBaseXp)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(baseXp);
        _baseXp = baseXp;
    }

    /// <inheritdoc />
    public long GetXpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 1);
        return (long)_baseXp * level * (level - 1) / 2;
    }

    /// <inheritdoc />
    public int GetLevelForXp(long xp)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(xp);

        // Обратная формула: n = (1 + √(1 + 8·xp / Base)) / 2, затем поправка на погрешность double
        var level = (int)Math.Floor((1 + Math.Sqrt(1 + 8.0 * xp / _baseXp)) / 2);
        while (GetXpForLevel(level + 1) <= xp)
            level++;
        while (level > 1 && GetXpForLevel(level) > xp)
            level--;
        return Math.Max(1, level);
    }

    /// <inheritdoc />
    public double GetProgressToNextLevel(long xp) => Describe(xp).Progress;

    /// <inheritdoc />
    public LevelProgress Describe(long xp)
    {
        var level = GetLevelForXp(xp);
        return new LevelProgress(level, xp, GetXpForLevel(level), GetXpForLevel(level + 1));
    }
}
