namespace Gamification.Core.Models;

/// <summary>Итог операции GrantXp фасада с учётом наград за достижения.</summary>
/// <param name="TotalXp">Суммарный опыт игрока.</param>
/// <param name="Level">Уровень после операции.</param>
/// <param name="LevelUps">На сколько уровней вырос игрок за операцию.</param>
/// <param name="Unlocked">Достижения, полученные за операцию.</param>
public sealed record GrantXpOutcome(long TotalXp, int Level, int LevelUps, IReadOnlyList<AchievementDefinition> Unlocked);
