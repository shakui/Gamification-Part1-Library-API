using Gamification.Core.Models;
using Gamification.Core.Rules;

namespace Gamification.Core.Tests;

public class PlayerProfileTests
{
    [Fact]
    public void NewPlayer_StartsAtLevelOneWithZeroXp()
    {
        var player = new PlayerProfile("  Alex  ");

        Assert.Equal("Alex", player.Nickname);
        Assert.Equal(1, player.Level);
        Assert.Equal(0, player.TotalXp);
        Assert.Empty(player.Achievements);
    }

    [Fact]
    public void PromoteTo_LowerLevel_Throws()
    {
        var player = new PlayerProfile("Alex");
        player.PromoteTo(3);

        Assert.Throws<ArgumentOutOfRangeException>(() => player.PromoteTo(2));
    }

    [Fact]
    public void Unlock_SameAchievementTwice_Throws()
    {
        var player = new PlayerProfile("Alex");
        var achievement = new AchievementDefinition("FIRST_STEPS", "Первые шаги", "", 10, new RegistrationRule());
        player.Unlock(achievement, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => player.Unlock(achievement, DateTime.UtcNow));
    }

    [Fact]
    public void AddXp_TransactionOfAnotherPlayer_Throws()
    {
        var player = new PlayerProfile("Alex");
        var foreign = new XpTransaction(Guid.NewGuid(), 10, "lesson");

        Assert.Throws<ArgumentException>(() => player.AddXp(foreign));
    }

    [Fact]
    public void CountActions_DoesNotCountAchievementRewards()
    {
        var player = new PlayerProfile("Alex");
        player.AddXp(new XpTransaction(player.Id, 10, "lesson"));
        player.AddXp(new XpTransaction(player.Id, 10, "purchase"));
        player.AddXp(new XpTransaction(player.Id, 10, XpSources.AchievementReward));

        Assert.Equal(2, player.CountActions());
        Assert.Equal(1, player.CountActions("lesson"));
        Assert.Equal(2, player.CountDistinctSources());
    }
}
