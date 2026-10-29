using Gamification.Core.Events;
using Gamification.Core.Models;
using Gamification.Core.Repositories;

namespace Gamification.Core.Services;

/// <summary>
/// Фасад игровой системы. Сам ничего не считает: вызывает репозиторий,
/// калькулятор опыта, сервис уровней и движок достижений.
/// </summary>
public sealed class GamificationEngine : IGamificationEngine
{
    private readonly IPlayerRepository _players;
    private readonly IXpCalculator _xpCalculator;
    private readonly ILevelService _levelService;
    private readonly IAchievementEvaluator _evaluator;
    private readonly object _sync = new(); // делает начисление опыта атомарным

    /// <summary>Создаёт фасад. Все зависимости — интерфейсы, их подставляет DI-контейнер.</summary>
    public GamificationEngine(IPlayerRepository players, IXpCalculator xpCalculator,
                              ILevelService levelService, IAchievementEvaluator evaluator)
    {
        _players = players ?? throw new ArgumentNullException(nameof(players));
        _xpCalculator = xpCalculator ?? throw new ArgumentNullException(nameof(xpCalculator));
        _levelService = levelService ?? throw new ArgumentNullException(nameof(levelService));
        _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
    }

    /// <inheritdoc />
    public event Action<IGameEvent>? GameEventOccurred;

    /// <inheritdoc />
    public PlayerProfile RegisterPlayer(string nickname)
    {
        var player = new PlayerProfile(nickname); // пустой ник → ArgumentException
        lock (_sync)
        {
            if (!_players.TryAdd(player))
                throw new InvalidOperationException("Игрок с таким Id уже существует.");
            ProcessEvents(player, [new PlayerRegisteredEvent(player.Id)]);
        }
        return player;
    }

    /// <inheritdoc />
    public GrantXpOutcome GrantXp(Guid playerId, long amount, string source)
    {
        if (source?.Trim() == XpSources.AchievementReward)
            throw new ArgumentException($"Источник «{XpSources.AchievementReward}» зарезервирован.", nameof(source));

        var player = GetProfile(playerId);
        lock (_sync)
        {
            var levelBefore = player.Level;
            var result = _xpCalculator.AddXp(player, amount, source!);
            var unlocked = ProcessEvents(player, result.Events);
            return new GrantXpOutcome(player.TotalXp, player.Level, player.Level - levelBefore, unlocked);
        }
    }

    /// <inheritdoc />
    public PlayerProfile GetProfile(Guid playerId) =>
        _players.GetById(playerId) ?? throw new KeyNotFoundException($"Игрок {playerId} не найден.");

    /// <inheritdoc />
    public LevelProgress GetProgress(Guid playerId) => _levelService.Describe(GetProfile(playerId).TotalXp);

    /// <inheritdoc />
    public IReadOnlyList<UnlockedAchievement> GetPlayerAchievements(Guid playerId)
    {
        var player = GetProfile(playerId);
        lock (_sync)
            return player.Achievements.ToList();
    }

    /// <inheritdoc />
    public IReadOnlyList<AchievementDefinition> GetAchievementCatalog() => _evaluator.Definitions;

    /// <inheritdoc />
    public IReadOnlyList<PlayerProfile> GetLeaderboard(int top)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(top, 1);
        return _players.GetAll()
            .OrderByDescending(p => p.TotalXp)
            .ThenBy(p => p.CreatedAt)
            .Take(top)
            .ToList();
    }

    // Обрабатывает очередь событий: публикует каждое и выдаёт заработанные достижения.
    // Награда за достижение — тоже опыт, она может дать новый уровень и новые достижения,
    // поэтому события от наград добавляются в ту же очередь.
    private List<AchievementDefinition> ProcessEvents(PlayerProfile player, IEnumerable<IGameEvent> initialEvents)
    {
        var unlocked = new List<AchievementDefinition>();
        var queue = new Queue<IGameEvent>(initialEvents);

        while (queue.Count > 0)
        {
            var gameEvent = queue.Dequeue();
            GameEventOccurred?.Invoke(gameEvent);

            foreach (var achievement in _evaluator.Evaluate(player, gameEvent))
            {
                if (player.HasAchievement(achievement.Code))
                    continue;
                Unlock(player, achievement, queue);
                unlocked.Add(achievement);
            }
        }
        return unlocked;
    }

    private void Unlock(PlayerProfile player, AchievementDefinition achievement, Queue<IGameEvent> queue)
    {
        player.Unlock(achievement, DateTime.UtcNow);
        queue.Enqueue(new AchievementUnlockedEvent(player.Id, achievement));

        if (achievement.XpReward <= 0)
            return;
        var reward = _xpCalculator.AddXp(player, achievement.XpReward, XpSources.AchievementReward);
        foreach (var rewardEvent in reward.Events)
            queue.Enqueue(rewardEvent);
    }
}
