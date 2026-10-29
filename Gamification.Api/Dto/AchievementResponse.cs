namespace Gamification.Api.Dto;

/// <summary>Достижение из каталога.</summary>
public class AchievementResponse
{
    /// <summary>Уникальный код.</summary>
    public string Code { get; set; } = "";

    /// <summary>Название.</summary>
    public string Title { get; set; } = "";

    /// <summary>Условие получения (у секретных скрыто).</summary>
    public string Description { get; set; } = "";

    /// <summary>Награда в опыте.</summary>
    public int XpReward { get; set; }

    /// <summary>Тип правила (у секретных скрыт).</summary>
    public string? RuleType { get; set; }

    /// <summary>Параметр правила (у секретных скрыт).</summary>
    public string? RuleParam { get; set; }

    /// <summary>Секретное достижение.</summary>
    public bool IsSecret { get; set; }
}
