namespace Gamification.Core.Models;

/// <summary>Прогресс игрока внутри текущего уровня.</summary>
/// <param name="CurrentLevel">Текущий уровень.</param>
/// <param name="CurrentXp">Суммарный опыт игрока.</param>
/// <param name="XpForCurrentLevel">Сколько опыта требовалось для текущего уровня.</param>
/// <param name="XpForNextLevel">Сколько опыта нужно для следующего уровня.</param>
public sealed record LevelProgress(int CurrentLevel, long CurrentXp, long XpForCurrentLevel, long XpForNextLevel)
{
    /// <summary>Сколько опыта осталось до следующего уровня.</summary>
    public long XpToNextLevel => XpForNextLevel - CurrentXp;

    /// <summary>Доля прохождения уровня, от 0 до 1.</summary>
    public double Progress => (double)(CurrentXp - XpForCurrentLevel) / (XpForNextLevel - XpForCurrentLevel);

    /// <summary>То же в процентах (0–100), округлено до десятых.</summary>
    public double Percent => Math.Round(Progress * 100, 1);
}
