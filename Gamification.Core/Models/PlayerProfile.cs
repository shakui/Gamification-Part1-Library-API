namespace Gamification.Core.Models;

/// <summary>
/// Профиль игрока. Состояние меняется только через методы
/// (AddXp, PromoteTo, Unlock); снаружи свойства доступны лишь для чтения.
/// </summary>
public sealed class PlayerProfile
{
    private readonly List<XpTransaction> _transactions = new();
    private readonly List<UnlockedAchievement> _achievements = new();

    /// <summary>Создаёт нового игрока с уровнем 1 и нулевым опытом.</summary>
    /// <exception cref="ArgumentException">Ник пустой.</exception>
    public PlayerProfile(string nickname)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nickname);

        Id = Guid.NewGuid();
        Nickname = nickname.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Идентификатор игрока.</summary>
    public Guid Id { get; }

    /// <summary>Ник.</summary>
    public string Nickname { get; }

    /// <summary>Суммарный опыт за всё время (никогда не уменьшается).</summary>
    public long TotalXp { get; private set; }

    /// <summary>Текущий уровень (начинается с 1).</summary>
    public int Level { get; private set; } = 1;

    /// <summary>Дата регистрации (UTC).</summary>
    public DateTime CreatedAt { get; }

    /// <summary>История начислений опыта.</summary>
    public IReadOnlyList<XpTransaction> Transactions => _transactions;

    /// <summary>Выданные достижения.</summary>
    public IReadOnlyList<UnlockedAchievement> Achievements => _achievements;

    /// <summary>Есть ли у игрока достижение с таким кодом.</summary>
    public bool HasAchievement(string code) => _achievements.Any(a => a.Achievement.Code == code);

    /// <summary>Сколько действий совершил игрок (награды за достижения не считаются).</summary>
    /// <param name="source">Источник; null — любые источники.</param>
    public int CountActions(string? source = null) =>
        _transactions.Count(t => !t.IsReward && (source is null || t.Source == source));

    /// <summary>Количество различных источников действий.</summary>
    public int CountDistinctSources() =>
        _transactions.Where(t => !t.IsReward).Select(t => t.Source).Distinct().Count();

    /// <summary>Добавляет начисление опыта.</summary>
    /// <exception cref="ArgumentException">Транзакция принадлежит другому игроку.</exception>
    public void AddXp(XpTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (transaction.PlayerId != Id)
            throw new ArgumentException("Транзакция принадлежит другому игроку.", nameof(transaction));

        _transactions.Add(transaction);
        TotalXp += transaction.Amount;
    }

    /// <summary>Повышает уровень. Понизить уровень нельзя.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Новый уровень ниже текущего.</exception>
    public void PromoteTo(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, Level);
        Level = level;
    }

    /// <summary>Выдаёт достижение. Повторно выдать нельзя.</summary>
    /// <exception cref="InvalidOperationException">Достижение уже выдано.</exception>
    public void Unlock(AchievementDefinition achievement, DateTime unlockedAt)
    {
        ArgumentNullException.ThrowIfNull(achievement);
        if (HasAchievement(achievement.Code))
            throw new InvalidOperationException($"Достижение {achievement.Code} уже выдано.");

        _achievements.Add(new UnlockedAchievement(achievement, unlockedAt));
    }
}
