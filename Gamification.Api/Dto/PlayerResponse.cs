namespace Gamification.Api.Dto;

/// <summary>Профиль игрока.</summary>
public class PlayerResponse
{
    /// <summary>Идентификатор игрока.</summary>
    public Guid Id { get; set; }

    /// <summary>Ник.</summary>
    public string Nickname { get; set; } = "";

    /// <summary>Суммарный опыт.</summary>
    public long TotalXp { get; set; }

    /// <summary>Уровень.</summary>
    public int Level { get; set; }

    /// <summary>Дата регистрации (UTC).</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Сколько достижений получено.</summary>
    public int AchievementsCount { get; set; }

    /// <summary>Прогресс до следующего уровня.</summary>
    public LevelProgressResponse Progress { get; set; } = new();
}
