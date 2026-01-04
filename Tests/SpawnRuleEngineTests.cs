using FeatherQuest.Core.Models;
using System;

namespace FeatherQuest.Tests;

/// <summary>
/// Unit tests for SpawnRuleEngine behavioral spawning logic.
/// Tests that WorldContext conditions trigger appropriate spawn patterns.
/// </summary>
[TestFixture]
public class SpawnRuleEngineTests
{
    private SpawnRuleEngine _engine;

    [SetUp]
    public void SetUp()
    {
        _engine = new SpawnRuleEngine();
    }

    #region Weather-Based Rules

    [Test]
    public void PostRain_TriggersSwarmBehavior()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Swarm));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(2.0f));
        Assert.That(modifier.GroupSize, Is.EqualTo(5));
    }

    [Test]
    public void HeavyRain_TriggersHiddenBehavior()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.HeavyRain,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Hidden));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.2f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void LightRain_ReducesSpawnRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.LightRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.5f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void Fog_SignificantlyReducesSpawnRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dawn,
            Weather = Weather.Fog,
            Season = Season.Fall
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.4f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void Snow_ReducesSpawnRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Snow,
            Season = Season.Winter
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.6f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void Windy_ReducesSpawnRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.Windy,
            Season = Season.Fall
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.7f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    #endregion

    #region Time of Day Rules

    [Test]
    public void Dawn_TriggersFlocksWithIncreasedRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dawn,
            Weather = Weather.Clear,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Flock));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.5f));
        Assert.That(modifier.GroupSize, Is.EqualTo(3));
    }

    [Test]
    public void Morning_TriggersFlocksWithIncreasedRate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.Clear,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Flock));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.5f));
        Assert.That(modifier.GroupSize, Is.EqualTo(3));
    }

    [Test]
    public void Midday_ReducesSpawnFrequency()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Clear,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.5f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void Dusk_TriggersSmallFlocksWithModerateIncrease()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dusk,
            Weather = Weather.Clear,
            Season = Season.Fall
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Flock));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.3f));
        Assert.That(modifier.GroupSize, Is.EqualTo(2));
    }

    [Test]
    public void Night_TriggersHiddenBehavior()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Night,
            Weather = Weather.Clear,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Hidden));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.3f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    #endregion

    #region Weather Priority Over Time

    [Test]
    public void PostRain_OverridesMorningFlockBehavior()
    {
        // Arrange - Morning would normally trigger flocks, but PostRain should trigger swarms
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Swarm));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(2.0f));
        Assert.That(modifier.GroupSize, Is.EqualTo(5));
    }

    [Test]
    public void HeavyRain_OverridesDawnFlockBehavior()
    {
        // Arrange - Dawn would normally trigger flocks, but HeavyRain should hide birds
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dawn,
            Weather = Weather.HeavyRain,
            Season = Season.Fall
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Hidden));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.2f));
    }

    #endregion

    #region Default and Edge Cases

    [Test]
    public void ClearAfternoon_ReturnsNormalBehavior()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.Clear,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.0f));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
    }

    [Test]
    public void PartlyCloudyMorning_TriggersFlocksAsNormal()
    {
        // Arrange - PartlyCloudy should not interfere with time-of-day rules
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PartlyCloudy,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Flock));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.5f));
        Assert.That(modifier.GroupSize, Is.EqualTo(3));
    }

    [Test]
    public void NullContext_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _engine.EvaluateSpawnRules(null!));
    }

    [Test]
    public void AllSeasons_ProducesValidModifiers()
    {
        // Test all seasons work properly
        foreach (Season season in Enum.GetValues(typeof(Season)))
        {
            var context = new WorldContext
            {
                TimeOfDay = TimeOfDay.Morning,
                Weather = Weather.Clear,
                Season = season
            };

            var modifier = _engine.EvaluateSpawnRules(context);

            Assert.That(modifier, Is.Not.Null);
            Assert.That(modifier.SpawnRateMultiplier, Is.GreaterThan(0));
            Assert.That(modifier.GroupSize, Is.GreaterThan(0));
        }
    }

    #endregion

    #region Realistic Scenarios

    [Test]
    public void PostRainSpringMorning_OptimalBirdingConditions()
    {
        // Arrange - This is when robins and thrushes come out to hunt worms
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Swarm));
        Assert.That(modifier.SpawnRateMultiplier, Is.GreaterThan(1.5f));
        Assert.That(modifier.GroupSize, Is.GreaterThanOrEqualTo(5));
    }

    [Test]
    public void FoggySummerMidday_WorstBirdingConditions()
    {
        // Arrange - Low visibility + low activity time
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Fog,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert - Fog takes priority and reduces spawns significantly
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.SpawnRateMultiplier, Is.LessThan(0.5f));
    }

    [Test]
    public void WinterNightClear_NocturnalBirdsOnly()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Night,
            Weather = Weather.Clear,
            Season = Season.Winter
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Hidden));
        Assert.That(modifier.SpawnRateMultiplier, Is.LessThanOrEqualTo(0.3f));
    }

    #endregion
}
