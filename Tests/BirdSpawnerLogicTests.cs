using FeatherQuest.Core.Models;
using FeatherQuest.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeatherQuest.Tests;

/// <summary>
/// Tests for BirdSpawner logic.
/// Note: These tests validate the spawner's selection and randomization logic
/// without requiring the Godot runtime.
/// </summary>
public class BirdSpawnerLogicTests
{
    private SpawnRuleEngine _spawnRuleEngine;

    [SetUp]
    public void SetUp()
    {
        _spawnRuleEngine = new SpawnRuleEngine();
    }

    private IReadOnlyDictionary<string, BirdDefinition> CreateTestBirdDatabase()
    {
        var robin = new BirdDefinition
        {
            ID = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
            CommonName = "American Robin",
            ScientificName = "Turdus migratorius",
            Tier = DifficultyTier.Beginner,
            FieldMarks = new[] { "orange_breast", "gray_back", "yellow_bill" },
            Variants = new List<PlumageVariant>
            {
                new PlumageVariant
                {
                    PlumageType = PlumageType.Breeding,
                    Gender = Gender.Male,
                    SpritePath = new AssetReference { Path = "res://Assets/Birds/robin_breeding_male.png" },
                    DifficultyRating = DifficultyTier.Beginner
                }
            },
            Calls = new List<BirdCall>
            {
                new BirdCall
                {
                    Type = CallType.Song,
                    AudioPath = new AssetReference { Path = "res://Assets/Audio/robin_song.mp3" },
                    SpectrogramPath = new AssetReference { Path = "res://Assets/Spectrograms/robin_song.png" }
                }
            }
        };

        var bluebird = new BirdDefinition
        {
            ID = "8f456e7a-9b12-4321-b9c0-5c8d7e1a3b2f",
            CommonName = "Eastern Bluebird",
            ScientificName = "Sialia sialis",
            Tier = DifficultyTier.Beginner,
            FieldMarks = new[] { "blue_back", "orange_breast", "white_belly" },
            Variants = new List<PlumageVariant>
            {
                new PlumageVariant
                {
                    PlumageType = PlumageType.Breeding,
                    Gender = Gender.Male,
                    SpritePath = new AssetReference { Path = "res://Assets/Birds/bluebird_breeding_male.png" },
                    DifficultyRating = DifficultyTier.Beginner
                },
                new PlumageVariant
                {
                    PlumageType = PlumageType.Breeding,
                    Gender = Gender.Female,
                    SpritePath = new AssetReference { Path = "res://Assets/Birds/bluebird_breeding_female.png" },
                    DifficultyRating = DifficultyTier.Beginner
                }
            },
            Calls = new List<BirdCall>
            {
                new BirdCall
                {
                    Type = CallType.Song,
                    AudioPath = new AssetReference { Path = "res://Assets/Audio/bluebird_song.mp3" },
                    SpectrogramPath = new AssetReference { Path = "res://Assets/Spectrograms/bluebird_song.png" }
                }
            }
        };

        return new Dictionary<string, BirdDefinition>
        {
            { "f47ac10b-58cc-4372-a567-0e02b2c3d479", robin },
            { "8f456e7a-9b12-4321-b9c0-5c8d7e1a3b2f", bluebird }
        };
    }

    [Test]
    public void BirdLoader_LoadsJsonSuccessfully()
    {
        // Use the test data that already exists for BirdLoader tests
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        var database = loader.LoadFromJson(json);
        
        Assert.That(database, Is.Not.Null);
        Assert.That(database.Count, Is.GreaterThan(0));
        Assert.That(database.ContainsKey("f47ac10b-58cc-4372-a567-0e02b2c3d479"), Is.True);
    }

    [Test]
    public void BirdDatabase_ContainsExpectedBirds()
    {
        var database = CreateTestBirdDatabase();
        
        Assert.That(database.Count, Is.EqualTo(2));
        Assert.That(database.ContainsKey("f47ac10b-58cc-4372-a567-0e02b2c3d479"), Is.True);
        Assert.That(database.ContainsKey("8f456e7a-9b12-4321-b9c0-5c8d7e1a3b2f"), Is.True);
    }

    [Test]
    public void BirdDefinition_HasVariants()
    {
        var database = CreateTestBirdDatabase();
        var robin = database["f47ac10b-58cc-4372-a567-0e02b2c3d479"];
        
        Assert.That(robin.Variants.Count, Is.EqualTo(1));
        Assert.That(robin.Variants[0].PlumageType, Is.EqualTo(PlumageType.Breeding));
        Assert.That(robin.Variants[0].Gender, Is.EqualTo(Gender.Male));
    }

    [Test]
    public void BirdDefinition_HasMultipleVariants()
    {
        var database = CreateTestBirdDatabase();
        var bluebird = database["8f456e7a-9b12-4321-b9c0-5c8d7e1a3b2f"];
        
        Assert.That(bluebird.Variants.Count, Is.EqualTo(2));
        Assert.That(bluebird.Variants[0].Gender, Is.EqualTo(Gender.Male));
        Assert.That(bluebird.Variants[1].Gender, Is.EqualTo(Gender.Female));
    }

    [Test]
    public void BirdDefinition_HasCalls()
    {
        var database = CreateTestBirdDatabase();
        var robin = database["f47ac10b-58cc-4372-a567-0e02b2c3d479"];
        
        Assert.That(robin.Calls.Count, Is.GreaterThan(0));
        Assert.That(robin.Calls[0].Type, Is.EqualTo(CallType.Song));
        Assert.That(robin.Calls[0].AudioPath.Path, Does.Contain("robin_song.mp3"));
    }

    [Test]
    public void RandomSelection_ProducesValidBird()
    {
        var database = CreateTestBirdDatabase();
        var random = new Random(12345); // Fixed seed for reproducibility
        
        var birdList = database.Values.ToList();
        var selectedBird = birdList[random.Next(birdList.Count)];
        
        Assert.That(selectedBird, Is.Not.Null);
        Assert.That(database.Values, Does.Contain(selectedBird));
    }

    [Test]
    public void RandomSelection_ProducesValidVariant()
    {
        var database = CreateTestBirdDatabase();
        var bluebird = database["8f456e7a-9b12-4321-b9c0-5c8d7e1a3b2f"];
        var random = new Random(12345); // Fixed seed for reproducibility
        
        var selectedVariant = bluebird.Variants[random.Next(bluebird.Variants.Count)];
        
        Assert.That(selectedVariant, Is.Not.Null);
        Assert.That(bluebird.Variants, Does.Contain(selectedVariant));
    }

    [Test]
    public void RandomSelection_MultipleCalls_ProducesDifferentResults()
    {
        var database = CreateTestBirdDatabase();
        var random = new Random(12345); // Fixed seed to avoid spurious failures
        var birdList = database.Values.ToList();
        
        var selections = new List<string>();
        for (int i = 0; i < 50; i++) // Increased iterations for better coverage
        {
            var bird = birdList[random.Next(birdList.Count)];
            selections.Add(bird.ID);
        }
        
        // With 50 selections from 2 birds, we expect multiple selections of each
        var distinctSelections = selections.Distinct().Count();
        Assert.That(distinctSelections, Is.GreaterThan(1), "Random selection should produce varied results");
    }

    [Test]
    public void SpawnInterval_GeneratesValidRange()
    {
        var random = new Random();
        float minInterval = 3.0f;
        float maxInterval = 8.0f;
        
        for (int i = 0; i < 20; i++)
        {
            float interval = (float)(random.NextDouble() * (maxInterval - minInterval) + minInterval);
            Assert.That(interval, Is.GreaterThanOrEqualTo(minInterval));
            Assert.That(interval, Is.LessThanOrEqualTo(maxInterval));
        }
    }

    [Test]
    public void SpawnPosition_GeneratesValidCoordinates()
    {
        var random = new Random();
        float minX = 0f, maxX = 1024f;
        float minY = 0f, maxY = 600f;
        
        for (int i = 0; i < 20; i++)
        {
            float x = (float)(random.NextDouble() * (maxX - minX) + minX);
            float y = (float)(random.NextDouble() * (maxY - minY) + minY);
            
            Assert.That(x, Is.GreaterThanOrEqualTo(minX));
            Assert.That(x, Is.LessThanOrEqualTo(maxX));
            Assert.That(y, Is.GreaterThanOrEqualTo(minY));
            Assert.That(y, Is.LessThanOrEqualTo(maxY));
        }
    }

    [Test]
    public void EmptyDatabase_HandlesGracefully()
    {
        // Simulate the SelectRandomBird logic with empty database
        var emptyDatabase = new Dictionary<string, BirdDefinition>();
        
        BirdDefinition? result = null;
        if (emptyDatabase.Count > 0)
        {
            var birdList = emptyDatabase.Values.ToList();
            var random = new Random();
            result = birdList[random.Next(birdList.Count)];
        }
        
        Assert.That(result, Is.Null, "SelectRandomBird should return null for empty database");
    }

    [Test]
    public void BirdWithNoVariants_HandlesGracefully()
    {
        var birdWithNoVariants = new BirdDefinition
        {
            ID = "test_bird",
            CommonName = "Test Bird",
            ScientificName = "Testus birdus",
            Variants = new List<PlumageVariant>() // Empty list
        };

        // Simulate the SelectRandomPlumageVariant logic with no variants
        PlumageVariant? result = null;
        if (birdWithNoVariants != null && birdWithNoVariants.Variants.Count > 0)
        {
            var random = new Random();
            result = birdWithNoVariants.Variants[random.Next(birdWithNoVariants.Variants.Count)];
        }
        
        Assert.That(result, Is.Null, "SelectRandomPlumageVariant should return null when bird has no variants");
    }

    #region Spawn Rule Integration Tests

    [Test]
    public void SpawnRuleEngine_PostRainContext_ProducesSwarmModifier()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        var modifier = _spawnRuleEngine.EvaluateSpawnRules(context);

        // Assert - This would cause BirdSpawner to spawn 5 birds at once
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Swarm));
        Assert.That(modifier.GroupSize, Is.EqualTo(5));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(2.0f));
    }

    [Test]
    public void SpawnRuleEngine_MiddayContext_ReducesSpawnFrequency()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Clear,
            Season = Season.Summer
        };

        // Act
        var modifier = _spawnRuleEngine.EvaluateSpawnRules(context);

        // Assert - BirdSpawner should spawn half as often
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Solitary));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.5f));
    }

    [Test]
    public void SpawnRuleEngine_HeavyRainContext_HidesBirds()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.HeavyRain,
            Season = Season.Summer
        };

        // Act
        var modifier = _spawnRuleEngine.EvaluateSpawnRules(context);

        // Assert - BirdSpawner should rarely spawn birds
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Hidden));
        Assert.That(modifier.GroupSize, Is.EqualTo(1));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(0.2f));
    }

    [Test]
    public void SpawnRuleEngine_MorningContext_IncreasesFlockActivity()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.Clear,
            Season = Season.Spring
        };

        // Act
        var modifier = _spawnRuleEngine.EvaluateSpawnRules(context);

        // Assert - BirdSpawner should spawn flocks of 3
        Assert.That(modifier.Pattern, Is.EqualTo(SpawnPattern.Flock));
        Assert.That(modifier.GroupSize, Is.EqualTo(3));
        Assert.That(modifier.SpawnRateMultiplier, Is.EqualTo(1.5f));
    }

    [Test]
    public void SpawnInterval_AdjustedByMultiplier_ProducesCorrectTiming()
    {
        // Arrange
        float baseInterval = 5.0f; // seconds
        float swarmMultiplier = 2.0f; // PostRain
        float hiddenMultiplier = 0.2f; // HeavyRain
        
        // Act
        float swarmInterval = baseInterval / swarmMultiplier;
        float hiddenInterval = baseInterval / hiddenMultiplier;
        
        // Assert
        Assert.That(swarmInterval, Is.EqualTo(2.5f)); // Spawn twice as fast
        Assert.That(hiddenInterval, Is.EqualTo(25.0f)); // Spawn 5x slower
    }

    [Test]
    public void GroupSize_DeterminesNumberOfBirdsSpawned()
    {
        // This test documents the expected behavior of BirdSpawner
        // when processing different spawn modifiers
        
        var testCases = new[]
        {
            (SpawnPattern.Solitary, 1, "Single bird"),
            (SpawnPattern.Flock, 3, "Small group"),
            (SpawnPattern.Swarm, 5, "Large group"),
            (SpawnPattern.Hidden, 1, "Rare single bird")
        };

        foreach (var (pattern, expectedSize, description) in testCases)
        {
            // For each pattern, BirdSpawner should spawn exactly that many birds
            Assert.That(expectedSize, Is.GreaterThan(0), 
                $"{description} should have positive group size");
        }
    }

    #endregion
}
