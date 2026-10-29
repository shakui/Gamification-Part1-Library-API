using Gamification.Core.Events;
using Gamification.Core.Models;
using Gamification.Core.Services;

namespace Gamification.Core.Tests;

public class XpCalculatorTests
{
    private readonly XpCalculator _calculator = new(new LevelService());

    [Fact]
    public void AddXp_AddsXpAndCreatesTransaction()
    {
        var player = new PlayerProfile("Alex");

        var result = _calculator.AddXp(player, 50, "lesson");

        Assert.Equal(50, player.TotalXp);
        Assert.Equal(50, result.TotalXp);
        Assert.Single(player.Transactions);
        Assert.Equal("lesson", result.Transaction.Source);
        Assert.Equal(1, player.Level);
    }

    [Fact]
    public void AddXp_LargeAmount_RaisesSeveralLevelsAtOnce()
    {
        var player = new PlayerProfile("Alex");
        var raised = new List<IGameEvent>();
        _calculator.EventRaised += raised.Add;

        var result = _calculator.AddXp(player, 1000, "purchase");

        Assert.Equal(5, player.Level);
        Assert.Equal(4, result.LevelUps);
        var levelUp = Assert.Single(raised.OfType<LevelUpEvent>());
        Assert.Equal(1, levelUp.OldLevel);
        Assert.Equal(5, levelUp.NewLevel);
        Assert.Single(raised.OfType<XpGainedEvent>());
    }

    [Fact]
    public void AddXp_WithoutLevelUp_RaisesOnlyXpGained()
    {
        var player = new PlayerProfile("Alex");

        var result = _calculator.AddXp(player, 10, "login");

        var single = Assert.Single(result.Events);
        Assert.IsType<XpGainedEvent>(single);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void AddXp_NotPositiveAmount_Throws(long amount)
    {
        var player = new PlayerProfile("Alex");

        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.AddXp(player, amount, "lesson"));
        Assert.Equal(0, player.TotalXp);
    }

    [Fact]
    public void AddXp_EmptySource_Throws()
    {
        var player = new PlayerProfile("Alex");

        Assert.Throws<ArgumentException>(() => _calculator.AddXp(player, 10, "  "));
    }
}
