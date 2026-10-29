using Gamification.Core.Models;

namespace Gamification.Core.Repositories;

/// <summary>Хранилище игроков. Сейчас в памяти, позже может стать базой данных.</summary>
public interface IPlayerRepository
{
    /// <summary>Добавляет игрока. Возвращает false, если игрок с таким Id уже есть.</summary>
    bool TryAdd(PlayerProfile player);

    /// <summary>Игрок по Id или null, если не найден.</summary>
    PlayerProfile? GetById(Guid id);

    /// <summary>Все игроки.</summary>
    IReadOnlyCollection<PlayerProfile> GetAll();
}
