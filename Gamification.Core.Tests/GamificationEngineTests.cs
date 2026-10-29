using Gamification.Core.Achievements;
using Gamification.Core.Events;
using Gamification.Core.Repositories;
using Gamification.Core.Services;

namespace Gamification.Core.Tests;

public class GamificationEngineTests
{
    private static GamificationEngine CreateEngine()
    {
        var levels = new LevelService();
        return new GamificationEngine(
            new InMemoryPlayerRepository(),
            new XpCalculator(levels),
            levels,
            new AchievementEvaluator(DefaultAchievementCatalog.Create()));
    }

    [Fact]
    public void RegisterPlayer_GivesFirstStepsAndItsReward()
    {
        var engine = CreateEngine();

        var player = engine.RegisterPlayer("Alex");

        Assert.True(player.HasAchievement("FIRST_STEPS"));
        Assert.Equal(10, player.TotalXp);   // награда FIRST_STEPS
        Assert.Equal(1, player.Level);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void RegisterPlayer_EmptyNickname_Throws(string nickname)
    {
        var engine = CreateEngine();

        Assert.Throws<ArgumentException>(() => engine.RegisterPlayer(nickname));
    }

    [Fact]
    public void GrantXp_UnknownPlayer_ThrowsKeyNotFound()
    {
        var engine = CreateEngine();

        Assert.Throws<KeyNotFoundException>(() => engine.GrantXp(Guid.NewGuid(), 10, "lesson"));
    }

    [Fact]
    public void GrantXp_ReservedSource_Throws()
    {
        var engine = CreateEngine();
        var player = engine.RegisterPlayer("Alex");

        Assert.Throws<ArgumentException>(() => engine.GrantXp(player.Id, 10, "achievement"));
    }

    [Fact]
    public void GrantXp_ReachesLevel2_UnlocksLevel2WithReward()
    {
        var engine = CreateEngine();
        var player = engine.RegisterPlayer("Alex");      // 10 XP

        var outcome = engine.GrantXp(player.Id, 90, "lesson"); // 100 XP → уровень 2, +25 за LEVEL_2

        Assert.Equal(2, outcome.Level);
        Assert.Equal(1, outcome.LevelUps);
        Assert.Contains(outcome.Unlocked, a => a.Code == "LEVEL_2");
        Assert.Equal(125, outcome.TotalXp);
    }

    [Fact]
    public void GrantXp_AchievementIsGivenOnlyOnce()
    {
        var engine = CreateEngine();
        var player = engine.RegisterPlayer("Alex");
        for (var i = 0; i < 10; i++)
            engine.GrantXp(player.Id, 5, "lesson");

        var eleventh = engine.GrantXp(player.Id, 5, "lesson");

        Assert.DoesNotContain(eleventh.Unlocked, a => a.Code == "DEDICATED");
        Assert.Single(engine.GetPlayerAchievements(player.Id), a => a.Achievement.Code == "DEDICATED");
    }

    [Fact]
    public void GetLeaderboard_ReturnsTopPlayersByXp()
    {
        var engine = CreateEngine();
        var weak = engine.RegisterPlayer("Weak");
        var strong = engine.RegisterPlayer("Strong");
        var middle = engine.RegisterPlayer("Middle");
        engine.GrantXp(strong.Id, 500, "lesson");
        engine.GrantXp(middle.Id, 50, "lesson");

        var top = engine.GetLeaderboard(2);

        Assert.Equal([strong.Id, middle.Id], top.Select(p => p.Id));
        Assert.DoesNotContain(top, p => p.Id == weak.Id);
    }

    [Fact]
    public void GetLeaderboard_TopLessThanOne_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateEngine().GetLeaderboard(0));
    }

    [Fact]
    public void GameEventOccurred_IsRaisedForLevelUp()
    {
        var engine = CreateEngine();
        var events = new List<IGameEvent>();
        engine.GameEventOccurred += events.Add;
        var player = engine.RegisterPlayer("Alex");

        engine.GrantXp(player.Id, 1000, "purchase");

        Assert.Contains(events, e => e is LevelUpEvent);
        Assert.Contains(events, e => e is AchievementUnlockedEvent { Achievement.Code: "BIG_DEAL" });
    }

    [Fact]
    public void GetProgress_ReturnsXpToNextLevel()
    {
        var engine = CreateEngine();
        var player = engine.RegisterPlayer("Alex");   // 10 XP

        var progress = engine.GetProgress(player.Id);

        Assert.Equal(1, progress.CurrentLevel);
        Assert.Equal(90, progress.XpToNextLevel);
    }
}
