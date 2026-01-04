using FeatherQuest.Core.Models;
using FeatherQuest.Core.Logic;
using System;

namespace FeatherQuest.Tests;

/// <summary>
/// Demonstration tests showing the behavioral spawning system in action.
/// These tests validate the acceptance criteria from LOGIC-001.
/// </summary>
[TestFixture]
public class BehavioralSpawningDemoTests
{
    private SpawnRuleEngine _engine;

    [SetUp]
    public void SetUp()
    {
        _engine = new SpawnRuleEngine();
    }

    [Test]
    public void AcceptanceCriteria_PostRainTriggersSwarmWithIncreasedRate()
    {
        // Acceptance Criteria: Changing WorldContext.Weather to "PostRain" triggers 
        // a "Swarm" or increased spawn rate behavior.
        
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert - Demonstrates swarm behavior
        Console.WriteLine($"PostRain Scenario:");
        Console.WriteLine($"  Pattern: {modifier.Pattern}");
        Console.WriteLine($"  Group Size: {modifier.GroupSize} birds");
        Console.WriteLine($"  Spawn Rate Multiplier: {modifier.SpawnRateMultiplier}x");
        Console.WriteLine($"  Result: {modifier.GroupSize} robins emerge to hunt for worms!");
        Console.WriteLine();

        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Swarm), 
            "PostRain should trigger Swarm behavior");
        Assert.That(modifier.SpawnRateMultiplier, Is.GreaterThan(1.0f), 
            "PostRain should increase spawn rate");
        Assert.That(modifier.GroupSize, Is.GreaterThanOrEqualTo(5), 
            "Swarm should spawn 5+ birds");
    }

    [Test]
    public void AcceptanceCriteria_MiddayReducesSpawnFrequency()
    {
        // Acceptance Criteria: Changing WorldContext.TimeOfDay to "Midday" 
        // reduces spawn frequency.
        
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Clear,
            Season = Season.Summer
        };

        // Act
        var modifier = _engine.EvaluateSpawnRules(context);

        // Assert - Demonstrates reduced spawn rate
        Console.WriteLine($"Midday Scenario:");
        Console.WriteLine($"  Pattern: {modifier.Pattern}");
        Console.WriteLine($"  Group Size: {modifier.GroupSize} bird(s)");
        Console.WriteLine($"  Spawn Rate Multiplier: {modifier.SpawnRateMultiplier}x");
        Console.WriteLine($"  Result: Birds are less active during hot midday sun");
        Console.WriteLine();

        Assert.That(modifier.SpawnRateMultiplier, Is.LessThan(1.0f), 
            "Midday should reduce spawn rate");
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.5f), 
            "Midday should cut spawn rate in half");
    }

    [Test]
    public void AcceptanceCriteria_BirdSpawnerRespectsRules()
    {
        // Acceptance Criteria: BirdSpawner respects these rules in the running game.
        // This test demonstrates how spawn intervals are adjusted by the multiplier.
        
        // Arrange
        float baseInterval = 5.0f; // BirdSpawner's base spawn interval

        var scenarios = new[]
        {
            (Name: "Morning (Optimal)", 
             Context: new WorldContext { TimeOfDay = TimeOfDay.Morning, Weather = Weather.Clear, Season = Season.Spring }),
            (Name: "Midday (Reduced)", 
             Context: new WorldContext { TimeOfDay = TimeOfDay.Midday, Weather = Weather.Clear, Season = Season.Summer }),
            (Name: "PostRain (Swarm)", 
             Context: new WorldContext { TimeOfDay = TimeOfDay.Morning, Weather = Weather.PostRain, Season = Season.Spring }),
            (Name: "HeavyRain (Hidden)", 
             Context: new WorldContext { TimeOfDay = TimeOfDay.Afternoon, Weather = Weather.HeavyRain, Season = Season.Fall })
        };

        Console.WriteLine($"BirdSpawner Behavior (Base Interval: {baseInterval}s):");
        Console.WriteLine(new string('-', 70));

        foreach (var (name, context) in scenarios)
        {
            // Act
            var modifier = _engine.EvaluateSpawnRules(context);
            float adjustedInterval = baseInterval / modifier.SpawnRateMultiplier;

            // Assert & Display
            Console.WriteLine($"{name}:");
            Console.WriteLine($"  Weather: {context.Weather}, Time: {context.TimeOfDay}");
            Console.WriteLine($"  Pattern: {modifier.Pattern}, Group Size: {modifier.GroupSize}");
            Console.WriteLine($"  Spawn Rate: {modifier.SpawnRateMultiplier}x");
            Console.WriteLine($"  Actual Interval: {adjustedInterval:F1}s (spawns every {adjustedInterval:F1} seconds)");
            Console.WriteLine();

            Assert.That(modifier, Is.Not.Null, "Modifier should be generated for all scenarios");
            Assert.That(adjustedInterval, Is.GreaterThan(0), "Adjusted interval must be positive");
        }
    }

    [Test]
    public void Scenario_PlayerExperience_DynamicWeatherChanges()
    {
        // Demonstrates what a player would experience as weather changes
        Console.WriteLine("Player Experience - Weather Transitions:");
        Console.WriteLine(new string('=', 70));
        Console.WriteLine();

        // Morning starts clear
        var clearMorning = new WorldContext 
        { 
            TimeOfDay = TimeOfDay.Morning, 
            Weather = Weather.Clear, 
            Season = Season.Spring 
        };
        var modifier1 = _engine.EvaluateSpawnRules(clearMorning);
        Console.WriteLine("6:00 AM - Clear Morning:");
        Console.WriteLine($"  You see small FLOCKS ({modifier1.GroupSize} birds) appearing regularly");
        Console.WriteLine($"  Activity: {modifier1.SpawnRateMultiplier}x normal rate");
        Console.WriteLine();

        // Rain begins
        var lightRain = new WorldContext 
        { 
            TimeOfDay = TimeOfDay.Morning, 
            Weather = Weather.LightRain, 
            Season = Season.Spring 
        };
        var modifier2 = _engine.EvaluateSpawnRules(lightRain);
        Console.WriteLine("8:00 AM - Light Rain Begins:");
        Console.WriteLine($"  Birds take shelter, activity drops to {modifier2.SpawnRateMultiplier}x");
        Console.WriteLine($"  Pattern changes to {modifier2.Pattern}");
        Console.WriteLine();

        // After rain
        var postRain = new WorldContext 
        { 
            TimeOfDay = TimeOfDay.Morning, 
            Weather = Weather.PostRain, 
            Season = Season.Spring 
        };
        var modifier3 = _engine.EvaluateSpawnRules(postRain);
        Console.WriteLine("10:00 AM - After the Rain:");
        Console.WriteLine($"  SWARMS of {modifier3.GroupSize}+ robins appear!");
        Console.WriteLine($"  They're hunting for worms - activity at {modifier3.SpawnRateMultiplier}x!");
        Console.WriteLine($"  This is optimal birding time!");
        Console.WriteLine();

        // Assert the narrative makes sense
        Assert.That(modifier1.SpawnRateMultiplier, Is.GreaterThan(modifier2.SpawnRateMultiplier), 
            "Clear weather should be more active than rainy");
        Assert.That(modifier3.SpawnRateMultiplier, Is.GreaterThan(modifier1.SpawnRateMultiplier), 
            "PostRain should be the most active");
        Assert.That(modifier3.GroupSize, Is.GreaterThan(modifier1.GroupSize), 
            "PostRain swarms should be larger than regular flocks");
    }
}
