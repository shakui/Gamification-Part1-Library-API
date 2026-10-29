using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>
/// Правило выдачи достижения. Новый тип правила добавляется новым классом,
/// который реализует этот интерфейс (принцип открытости/закрытости).
/// </summary>
public interface IAchievementRule
{
    /// <summary>Название типа правила, например «LevelReached».</summary>
    string RuleType { get; }

    /// <summary>Параметр правила строкой, например «5» или «lesson:10».</summary>
    string Parameter { get; }

    /// <summary>Выполнено ли условие для игрока после события.</summary>
    bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent);

    /// <summary>Доля выполнения условия, от 0 до 1.</summary>
    double GetProgress(PlayerProfile player);
}
