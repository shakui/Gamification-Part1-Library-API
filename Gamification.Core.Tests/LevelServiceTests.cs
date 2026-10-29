using Gamification.Core.Services;

namespace Gamification.Core.Tests;

public class LevelServiceTests
{
    private readonly LevelService _service = new();

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 100)]
    [InlineData(3, 300)]
    [InlineData(4, 600)]
    [InlineData(5, 1000)]
    [InlineData(10, 4500)]
    public void GetXpForLevel_FollowsQuadraticFormula(int level, long expectedXp)
    {
        Assert.Equal(expectedXp, _service.GetXpForLevel(level));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(99, 1)]     // на 1 XP меньше границы
    [InlineData(100, 2)]    // ровно граница
    [InlineData(299, 2)]
    [InlineData(300, 3)]
    [InlineData(999, 4)]
    [InlineData(1000, 5)]
    [InlineData(4500, 10)]
    public void GetLevelForXp_ReturnsCorrectLevel_IncludingBoundaries(long xp, int expectedLevel)
    {
        Assert.Equal(expectedLevel, _service.GetLevelForXp(xp));
    }

    [Theory]
    [InlineData(100, 0.0)]   // только что получил 2-й уровень
    [InlineData(200, 0.5)]   // половина пути от 100 до 300
    [InlineData(250, 0.75)]
    public void GetProgressToNextLevel_ReturnsShareOfCurrentLevel(long xp, double expected)
    {
        Assert.Equal(expected, _service.GetProgressToNextLevel(xp), precision: 6);
    }

    [Fact]
    public void Describe_ReturnsXpToNextLevelAndPercent()
    {
        var progress = _service.Describe(250);

        Assert.Equal(2, progress.CurrentLevel);
        Assert.Equal(300, progress.XpForNextLevel);
        Assert.Equal(50, progress.XpToNextLevel);
        Assert.Equal(75.0, progress.Percent);
    }

    [Fact]
    public void GetXpForLevel_LevelBelowOne_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetXpForLevel(0));
    }

    [Fact]
    public void GetLevelForXp_NegativeXp_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetLevelForXp(-1));
    }
}
