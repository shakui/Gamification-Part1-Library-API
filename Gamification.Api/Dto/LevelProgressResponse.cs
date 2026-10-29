namespace Gamification.Api.Dto;

/// <summary>Прогресс до следующего уровня.</summary>
public class LevelProgressResponse
{
    /// <summary>Текущий уровень.</summary>
    public int CurrentLevel { get; set; }

    /// <summary>Суммарный опыт игрока.</summary>
    public long CurrentXp { get; set; }

    /// <summary>Опыт, с которого начинается текущий уровень.</summary>
    public long XpForCurrentLevel { get; set; }

    /// <summary>Опыт, нужный для следующего уровня.</summary>
    public long XpForNextLevel { get; set; }

    /// <summary>Сколько опыта осталось до следующего уровня.</summary>
    public long XpToNextLevel { get; set; }

    /// <summary>Процент прохождения уровня (0–100).</summary>
    public double Percent { get; set; }
}
