using Gamification.Core.Achievements;
using Gamification.Core.Events;
using Gamification.Core.Models;
using Gamification.Core.Rules;
using Gamification.Core.Services;

namespace Gamification.Core.Tests;

public class AchievementEvaluatorTests
{
    private static readonly AchievementDefinition Level2 =
        new("LEVEL_2", "Новичок", "Достигнут 2-й уровень", 25, new LevelReachedRule(2));

    [Fact]
    public void Evaluate_RuleSatisfied_ReturnsAchievement()
    {
        var evaluator = new AchievementEvaluator([Level2]);
        var player = new PlayerProfile("Alex");
        player.PromoteTo(2);

        var result = evaluator.Evaluate(player, new LevelUpEvent(player.Id, 1, 2));

        Assert.Equal("LEVEL_2", Assert.Single(result).Code);
    }

    [Fact]
    public void Evaluate_AlreadyUnlocked_ReturnsNothing()
    {
        var evaluator = new AchievementEvaluator([Level2]);
        var player = new PlayerProfile("Alex");
        player.PromoteTo(2);
        player.Unlock(Level2, DateTime.UtcNow);

        var result = evaluator.Evaluate(player, new LevelUpEvent(player.Id, 1, 2));

        Assert.Empty(result);
    }

    [Fact]
    public void Evaluate_RuleNotSatisfied_ReturnsNothing()
    {
        var evaluator = new AchievementEvaluator([Level2]);
        var player = new PlayerProfile("Alex");

        Assert.Empty(evaluator.Evaluate(player, new PlayerRegisteredEvent(player.Id)));
    }

    [Fact]
    public void Evaluate_EventOfAnotherPlayer_Throws()
    {
        var evaluator = new AchievementEvaluator([Level2]);
        var player = new PlayerProfile("Alex");

        Assert.Throws<ArgumentException>(() => evaluator.Evaluate(player, new PlayerRegisteredEvent(Guid.NewGuid())));
    }

    [Fact]
    public void Constructor_DuplicateCodes_Throws()
    {
        Assert.Throws<ArgumentException>(() => new AchievementEvaluator([Level2, Level2]));
    }

    [Fact]
    public void DefaultCatalog_HasAtLeastTenUniqueAchievements()
    {
        var catalog = DefaultAchievementCatalog.Create();

        Assert.True(catalog.Count >= 10);
        Assert.Equal(catalog.Count, catalog.Select(a => a.Code).Distinct().Count());
        Assert.Contains(catalog, a => a.IsSecret);
    }

    [Fact]
    public void SingleGrantRule_IgnoresAchievementRewards()
    {
        var rule = new SingleGrantRule(1000);
        var player = new PlayerProfile("Alex");

        Assert.True(rule.IsSatisfied(player, new XpGainedEvent(player.Id, 1000, "purchase", 1000)));
        Assert.False(rule.IsSatisfied(player, new XpGainedEvent(player.Id, 1000, XpSources.AchievementReward, 1000)));
    }
}
