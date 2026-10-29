using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>Движок достижений: проверяет правила каталога.</summary>
public interface IAchievementEvaluator
{
    /// <summary>Каталог всех достижений.</summary>
    IReadOnlyList<AchievementDefinition> Definitions { get; }

    /// <summary>
    /// Проверяет правила после события и возвращает достижения, которые игрок
    /// заработал и которых у него ещё нет. Уже выданные достижения не возвращаются.
    /// Сам метод ничего не выдаёт: выдачу делает вызывающий код.
    /// </summary>
    /// <param name="player">Игрок.</param>
    /// <param name="gameEvent">Событие, после которого идёт проверка.</param>
    /// <exception cref="ArgumentException">Событие относится к другому игроку.</exception>
    IReadOnlyList<AchievementDefinition> Evaluate(PlayerProfile player, IGameEvent gameEvent);
}
