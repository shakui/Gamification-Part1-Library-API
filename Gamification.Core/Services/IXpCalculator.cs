using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Services;

/// <summary>Начисление опыта игроку с пересчётом уровня.</summary>
public interface IXpCalculator
{
    /// <summary>Возникает при каждом событии начисления (XpGained, LevelUp).</summary>
    event Action<IGameEvent>? EventRaised;

    /// <summary>
    /// Начисляет опыт: создаёт транзакцию, добавляет опыт, пересчитывает уровень
    /// и поднимает события XpGained и (при повышении) LevelUp.
    /// </summary>
    /// <param name="player">Игрок.</param>
    /// <param name="amount">Опыт, больше нуля.</param>
    /// <param name="source">Источник опыта, непустой.</param>
    /// <returns>Транзакция, уровни до и после, события.</returns>
    /// <exception cref="ArgumentOutOfRangeException">amount меньше или равен нулю.</exception>
    /// <exception cref="ArgumentException">source пустой.</exception>
    XpGrantResult AddXp(PlayerProfile player, long amount, string source);
}
