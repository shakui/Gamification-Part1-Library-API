using Gamification.Core.Models;
using Gamification.Core.Rules;

namespace Gamification.Core.Achievements;

/// <summary>Стандартный каталог из 12 достижений (10 обязательных по ТЗ + 2 собственных).</summary>
public static class DefaultAchievementCatalog
{
    /// <summary>Создаёт новый список достижений.</summary>
    public static IReadOnlyList<AchievementDefinition> Create() => new List<AchievementDefinition>
    {
        new("FIRST_STEPS", "Первые шаги", "Игрок зарегистрирован", 10, new RegistrationRule()),
        new("LEVEL_2", "Новичок", "Достигнут 2-й уровень", 25, new LevelReachedRule(2)),
        new("LEVEL_5", "Опытный", "Достигнут 5-й уровень", 100, new LevelReachedRule(5)),
        new("LEVEL_10", "Ветеран", "Достигнут 10-й уровень", 300, new LevelReachedRule(10)),
        new("COLLECTOR_500", "Копилка", "Набрано суммарно 500 XP", 50, new TotalXpRule(500)),
        new("COLLECTOR_5000", "Сокровищница", "Набрано суммарно 5000 XP", 250, new TotalXpRule(5000)),
        new("MARATHON_7", "Марафон", "7 действий с любым источником", 40, new ActionCountRule(7)),
        new("DEDICATED", "Прилежный ученик", "10 действий с источником «lesson»", 60,
            new ActionCountRule(10, "lesson")),
        new("SHOPAHOLIC", "Шопоголик", "5 действий с источником «purchase»", 50,
            new ActionCountRule(5, "purchase")),
        new("SECRET", "Мастер на все руки", "Достигнут 7-й уровень и использовано не меньше 3 источников", 200,
            new LevelAndSourcesRule(7, 3), isSecret: true),

        // Собственные достижения
        new("REGULAR", "Завсегдатай", "3 действия с источником «login»", 15, new ActionCountRule(3, "login")),
        new("BIG_DEAL", "Крупная сделка", "За одно действие получено не меньше 1000 XP", 75, new SingleGrantRule(1000)),
    };
}
