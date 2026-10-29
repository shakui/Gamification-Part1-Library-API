using Gamification.Core.Events;
using Gamification.Core.Models;

namespace Gamification.Core.Rules;

/// <summary>Выполняется сразу после регистрации игрока.</summary>
public sealed class RegistrationRule : IAchievementRule
{
    /// <inheritdoc />
    public string RuleType => "Registration";

    /// <inheritdoc />
    public string Parameter => "";

    /// <inheritdoc />
    public bool IsSatisfied(PlayerProfile player, IGameEvent gameEvent) => true;

    /// <inheritdoc />
    public double GetProgress(PlayerProfile player) => 1;
}
