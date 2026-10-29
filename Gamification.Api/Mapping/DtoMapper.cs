using Gamification.Api.Dto;
using Gamification.Core.Models;

namespace Gamification.Api.Mapping;

/// <summary>Преобразование доменных объектов библиотеки в DTO ответов.</summary>
public static class DtoMapper
{
    private const string HiddenTitle = "Секретное достижение";
    private const string HiddenDescription = "Условие скрыто. Продолжайте играть!";

    /// <summary>Профиль игрока + прогресс → PlayerResponse.</summary>
    public static PlayerResponse ToResponse(this PlayerProfile player, LevelProgress progress) => new()
    {
        Id = player.Id,
        Nickname = player.Nickname,
        TotalXp = player.TotalXp,
        Level = player.Level,
        CreatedAt = player.CreatedAt,
        AchievementsCount = player.Achievements.Count,
        Progress = progress.ToResponse()
    };

    /// <summary>LevelProgress → LevelProgressResponse.</summary>
    public static LevelProgressResponse ToResponse(this LevelProgress progress) => new()
    {
        CurrentLevel = progress.CurrentLevel,
        CurrentXp = progress.CurrentXp,
        XpForCurrentLevel = progress.XpForCurrentLevel,
        XpForNextLevel = progress.XpForNextLevel,
        XpToNextLevel = progress.XpToNextLevel,
        Percent = progress.Percent
    };

    /// <summary>
    /// Достижение → AchievementResponse. У секретного достижения условие скрывается,
    /// пока revealSecret = false (в каталоге). Полученное игроком показывается полностью.
    /// </summary>
    public static AchievementResponse ToResponse(this AchievementDefinition achievement, bool revealSecret = false)
    {
        var hide = achievement.IsSecret && !revealSecret;
        return new AchievementResponse
        {
            Code = achievement.Code,
            Title = hide ? HiddenTitle : achievement.Title,
            Description = hide ? HiddenDescription : achievement.Description,
            XpReward = achievement.XpReward,
            RuleType = hide ? null : achievement.RuleType,
            RuleParam = hide ? null : achievement.RuleParam,
            IsSecret = achievement.IsSecret
        };
    }

    /// <summary>Выданное достижение → PlayerAchievementResponse.</summary>
    public static PlayerAchievementResponse ToResponse(this UnlockedAchievement unlocked) => new()
    {
        Code = unlocked.Achievement.Code,
        Title = unlocked.Achievement.Title,
        Description = unlocked.Achievement.Description,
        XpReward = unlocked.Achievement.XpReward,
        UnlockedAt = unlocked.UnlockedAt
    };

    /// <summary>Итог начисления → GrantXpResponse.</summary>
    public static GrantXpResponse ToResponse(this GrantXpOutcome outcome) => new()
    {
        TotalXp = outcome.TotalXp,
        Level = outcome.Level,
        LevelUps = outcome.LevelUps,
        Unlocked = outcome.Unlocked.Select(a => a.ToResponse(revealSecret: true)).ToList()
    };

    /// <summary>Игрок и его место → строка рейтинга.</summary>
    public static LeaderboardEntryResponse ToLeaderboardEntry(this PlayerProfile player, int rank) => new()
    {
        Rank = rank,
        PlayerId = player.Id,
        Nickname = player.Nickname,
        Level = player.Level,
        TotalXp = player.TotalXp
    };
}
