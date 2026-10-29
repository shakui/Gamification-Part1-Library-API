using Gamification.Core.Rules;

namespace Gamification.Core.Models;

/// <summary>Описание достижения из каталога: что нужно сделать и какая награда.</summary>
public sealed class AchievementDefinition
{
    /// <summary>Создаёт описание достижения.</summary>
    public AchievementDefinition(string code, string title, string description,
                                 int xpReward, IAchievementRule rule, bool isSecret = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentOutOfRangeException.ThrowIfNegative(xpReward);
        ArgumentNullException.ThrowIfNull(rule);

        Code = code;
        Title = title;
        Description = description;
        XpReward = xpReward;
        Rule = rule;
        IsSecret = isSecret;
    }

    /// <summary>Уникальный код, например «LEVEL_5».</summary>
    public string Code { get; }

    /// <summary>Название.</summary>
    public string Title { get; }

    /// <summary>Описание условия.</summary>
    public string Description { get; }

    /// <summary>Опыт, который игрок получает за достижение.</summary>
    public int XpReward { get; }

    /// <summary>Правило выдачи.</summary>
    public IAchievementRule Rule { get; }

    /// <summary>Тип правила (для хранения в БД).</summary>
    public string RuleType => Rule.RuleType;

    /// <summary>Параметр правила (для хранения в БД).</summary>
    public string RuleParam => Rule.Parameter;

    /// <summary>Секретное достижение: условие не показывается в каталоге.</summary>
    public bool IsSecret { get; }
}
