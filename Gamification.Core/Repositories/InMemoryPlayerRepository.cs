using System.Collections.Concurrent;
using Gamification.Core.Models;

namespace Gamification.Core.Repositories;

/// <summary>Потокобезопасное хранилище игроков в оперативной памяти.</summary>
public sealed class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ConcurrentDictionary<Guid, PlayerProfile> _players = new();

    /// <inheritdoc />
    public bool TryAdd(PlayerProfile player)
    {
        ArgumentNullException.ThrowIfNull(player);
        return _players.TryAdd(player.Id, player);
    }

    /// <inheritdoc />
    public PlayerProfile? GetById(Guid id) => _players.GetValueOrDefault(id);

    /// <inheritdoc />
    public IReadOnlyCollection<PlayerProfile> GetAll() => _players.Values.ToList();
}
