namespace Gamification.Core.Models;

/// <summary>Зарезервированные источники опыта.</summary>
public static class XpSources
{
    /// <summary>
    /// Источник опыта, начисляемого как награда за достижение.
    /// Такие начисления не считаются «действиями» игрока.
    /// </summary>
    public const string AchievementReward = "achievement";
}
