namespace Gamification.Core.Models;

/// <summary>Одно начисление опыта игроку.</summary>
public sealed class XpTransaction
{
    /// <summary>Создаёт транзакцию. Опыт должен быть больше нуля, источник — непустым.</summary>
    /// <exception cref="ArgumentOutOfRangeException">amount меньше или равен нулю.</exception>
    /// <exception cref="ArgumentException">source пустой.</exception>
    public XpTransaction(Guid playerId, long amount, string source)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        ArgumentException.ThrowIfNullOrWhiteSpace(source);

        Id = Guid.NewGuid();
        PlayerId = playerId;
        Amount = amount;
        Source = source.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Идентификатор транзакции.</summary>
    public Guid Id { get; }

    /// <summary>Игрок, которому начислен опыт.</summary>
    public Guid PlayerId { get; }

    /// <summary>Количество опыта (больше нуля).</summary>
    public long Amount { get; }

    /// <summary>Источник опыта: «lesson», «purchase» и т. п.</summary>
    public string Source { get; }

    /// <summary>Время начисления (UTC).</summary>
    public DateTime CreatedAt { get; }

    /// <summary>true — это награда за достижение, а не действие игрока.</summary>
    public bool IsReward => Source == XpSources.AchievementReward;
}
