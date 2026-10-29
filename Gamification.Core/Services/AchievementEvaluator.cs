using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>Проверяет правила достижений. Каталог передаётся через конструктор.</summary>
public sealed class AchievementEvaluator : IAchievementEvaluator
{
    private readonly List<AchievementDefinition> _definitions;

    /// <summary>Создаёт движок с каталогом достижений.</summary>
    /// <exception cref="ArgumentException">В каталоге повторяется код достижения.</exception>
    public AchievementEvaluator(IEnumerable<AchievementDefinition> catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        _definitions = catalog.ToList();

        var duplicate = _definitions.GroupBy(d => d.Code).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Код достижения {duplicate.Key} повторяется.", nameof(catalog));
    }

    /// <inheritdoc />
    public IReadOnlyList<AchievementDefinition> Definitions => _definitions;

    /// <inheritdoc />
    public IReadOnlyList<AchievementDefinition> Evaluate(PlayerProfile player, IGameEvent gameEvent)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(gameEvent);
        if (gameEvent.PlayerId != player.Id)
            throw new ArgumentException("Событие относится к другому игроку.", nameof(gameEvent));

        return _definitions
            .Where(d => !player.HasAchievement(d.Code) && d.Rule.IsSatisfied(player, gameEvent))
            .ToList();
    }
}
