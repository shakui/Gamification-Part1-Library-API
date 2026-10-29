namespace Gamification.Core.Models;

/// <summary>Достижение, выданное игроку (связь Player — Achievement).</summary>
/// <param name="Achievement">Какое достижение выдано.</param>
/// <param name="UnlockedAt">Когда выдано (UTC).</param>
public sealed record UnlockedAchievement(AchievementDefinition Achievement, DateTime UnlockedAt);
