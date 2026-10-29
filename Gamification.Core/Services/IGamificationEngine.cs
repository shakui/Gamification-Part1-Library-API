using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>Фасад игровой системы: единая точка входа для внешнего кода (например, Web API).</summary>
public interface IGamificationEngine
{
    /// <summary>Возникает при любом игровом событии (регистрация, опыт, уровень, достижение).</summary>
    event Action<IGameEvent>? GameEventOccurred;

    /// <summary>Регистрирует игрока и сразу проверяет достижения (например, FIRST_STEPS).</summary>
    /// <exception cref="ArgumentException">Ник пустой.</exception>
    PlayerProfile RegisterPlayer(string nickname);

    /// <summary>Начисляет опыт, пересчитывает уровень, выдаёт достижения и награды за них.</summary>
    /// <exception cref="KeyNotFoundException">Игрок не найден.</exception>
    /// <exception cref="ArgumentException">Опыт меньше или равен нулю, источник пустой или зарезервирован.</exception>
    GrantXpOutcome GrantXp(Guid playerId, long amount, string source);

    /// <summary>Профиль игрока.</summary>
    /// <exception cref="KeyNotFoundException">Игрок не найден.</exception>
    PlayerProfile GetProfile(Guid playerId);

    /// <summary>Прогресс игрока до следующего уровня.</summary>
    /// <exception cref="KeyNotFoundException">Игрок не найден.</exception>
    LevelProgress GetProgress(Guid playerId);

    /// <summary>Достижения игрока с датами выдачи.</summary>
    /// <exception cref="KeyNotFoundException">Игрок не найден.</exception>
    IReadOnlyList<UnlockedAchievement> GetPlayerAchievements(Guid playerId);

    /// <summary>Каталог всех достижений.</summary>
    IReadOnlyList<AchievementDefinition> GetAchievementCatalog();

    /// <summary>top игроков с наибольшим суммарным опытом.</summary>
    /// <exception cref="ArgumentOutOfRangeException">top меньше 1.</exception>
    IReadOnlyList<PlayerProfile> GetLeaderboard(int top);
}
