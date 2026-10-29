using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>Начисляет опыт и пересчитывает уровень через ILevelService.</summary>
public sealed class XpCalculator : IXpCalculator
{
    private readonly ILevelService _levelService;

    /// <summary>Создаёт калькулятор опыта.</summary>
    public XpCalculator(ILevelService levelService)
    {
        _levelService = levelService ?? throw new ArgumentNullException(nameof(levelService));
    }

    /// <inheritdoc />
    public event Action<IGameEvent>? EventRaised;

    /// <inheritdoc />
    public XpGrantResult AddXp(PlayerProfile player, long amount, string source)
    {
        ArgumentNullException.ThrowIfNull(player);

        var transaction = new XpTransaction(player.Id, amount, source); // здесь проверяются amount и source
        var oldLevel = player.Level;

        player.AddXp(transaction);
        var newLevel = _levelService.GetLevelForXp(player.TotalXp);
        if (newLevel > oldLevel)
            player.PromoteTo(newLevel);

        var events = BuildEvents(player, transaction, oldLevel, newLevel);
        foreach (var gameEvent in events)
            EventRaised?.Invoke(gameEvent);

        return new XpGrantResult(transaction, oldLevel, newLevel, player.TotalXp, events);
    }

    private static List<IGameEvent> BuildEvents(PlayerProfile player, XpTransaction transaction,
                                                int oldLevel, int newLevel)
    {
        var events = new List<IGameEvent>
        {
            new XpGainedEvent(player.Id, transaction.Amount, transaction.Source, player.TotalXp)
        };
        if (newLevel > oldLevel)
            events.Add(new LevelUpEvent(player.Id, oldLevel, newLevel));
        return events;
    }
}
