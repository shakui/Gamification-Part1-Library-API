using Gamification.Core.Achievements;
using Gamification.Core.Repositories;
using Gamification.Core.Services;

namespace Gamification.Api.Extensions;

/// <summary>Регистрация библиотеки Gamification.Core в DI-контейнере.</summary>
public static class GamificationServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы библиотеки. Все они Singleton:
    /// хранилище в памяти должно жить всё время работы приложения,
    /// а сервисы не хранят данных запроса, поэтому один экземпляр безопасно делить.
    /// </summary>
    public static IServiceCollection AddGamification(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        services.AddSingleton<ILevelService>(_ => new LevelService(LevelService.DefaultBaseXp));
        services.AddSingleton<IXpCalculator, XpCalculator>();
        services.AddSingleton<IAchievementEvaluator>(_ => new AchievementEvaluator(DefaultAchievementCatalog.Create()));
        services.AddSingleton<IGamificationEngine, GamificationEngine>();
        return services;
    }
}
