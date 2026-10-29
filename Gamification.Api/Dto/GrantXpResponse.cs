namespace Gamification.Api.Dto;

/// <summary>Результат начисления опыта.</summary>
public class GrantXpResponse
{
    /// <summary>Суммарный опыт после начисления (с наградами за достижения).</summary>
    public long TotalXp { get; set; }

    /// <summary>Уровень после начисления.</summary>
    public int Level { get; set; }

    /// <summary>На сколько уровней вырос игрок.</summary>
    public int LevelUps { get; set; }

    /// <summary>Новые достижения, полученные за это начисление.</summary>
    public List<AchievementResponse> Unlocked { get; set; } = new();
}
