namespace Gamification.Api.Dto;

/// <summary>Строка рейтинга игроков.</summary>
public class LeaderboardEntryResponse
{
    /// <summary>Место в рейтинге (с 1).</summary>
    public int Rank { get; set; }

    /// <summary>Идентификатор игрока.</summary>
    public Guid PlayerId { get; set; }

    /// <summary>Ник.</summary>
    public string Nickname { get; set; } = "";

    /// <summary>Уровень.</summary>
    public int Level { get; set; }

    /// <summary>Суммарный опыт.</summary>
    public long TotalXp { get; set; }
}
