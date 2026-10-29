namespace Gamification.Api.Dto;

/// <summary>Достижение, полученное игроком.</summary>
public class PlayerAchievementResponse
{
    /// <summary>Код достижения.</summary>
    public string Code { get; set; } = "";

    /// <summary>Название.</summary>
    public string Title { get; set; } = "";

    /// <summary>Условие получения.</summary>
    public string Description { get; set; } = "";

    /// <summary>Полученная награда в опыте.</summary>
    public int XpReward { get; set; }

    /// <summary>Дата выдачи (UTC).</summary>
    public DateTime UnlockedAt { get; set; }
}
