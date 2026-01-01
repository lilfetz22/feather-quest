using FeatherQuest.Core.Models;
using Newtonsoft.Json;

namespace FeatherQuest.Tests;

/// <summary>
/// Unit tests for WorldContext environmental condition modeling.
/// </summary>
[TestFixture]
public class WorldContextTests
{
    [Test]
    public void WorldContext_ClearMorning_FullVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.Clear,
            Season = Season.Spring
        };

        // Act
        context.RecalculateFactors();

        // Assert
        Assert.That(context.Visibility, Is.EqualTo(1.0f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.EqualTo(1.0f).Within(0.1f));
    }

    [Test]
    public void WorldContext_FoggyDusk_LowVisibilityHighMultiplier()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dusk,
            Weather = Weather.Fog,
            Season = Season.Fall
        };

        // Act
        context.RecalculateFactors();

        // Assert
        Assert.That(context.Visibility, Is.LessThan(0.3f));
        Assert.That(context.DifficultyMultiplier, Is.GreaterThan(1.5f));
    }

    [Test]
    public void WorldContext_NightTime_VeryLowVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Night,
            Weather = Weather.Clear,
            Season = Season.Winter
        };

        // Act
        context.RecalculateFactors();

        // Assert
        Assert.That(context.Visibility, Is.EqualTo(0.2f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.GreaterThan(1.5f));
    }

    [Test]
    public void WorldContext_HeavyRainAtDawn_ModerateVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dawn,
            Weather = Weather.HeavyRain,
            Season = Season.Spring
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Dawn = 0.6f, HeavyRain = 0.5f, combined = 0.3f
        Assert.That(context.Visibility, Is.EqualTo(0.3f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.GreaterThan(1.5f));
    }

    [Test]
    public void WorldContext_PartlyCloudyAfternoon_GoodVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.PartlyCloudy,
            Season = Season.Summer
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Afternoon = 0.9f, PartlyCloudy = 1.0f, combined = 0.9f
        Assert.That(context.Visibility, Is.EqualTo(0.9f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.EqualTo(1.1f).Within(0.1f));
    }

    [Test]
    public void WorldContext_MiddayInSnow_ReducedVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.Snow,
            Season = Season.Winter
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Midday = 1.0f, Snow = 0.6f, combined = 0.6f
        Assert.That(context.Visibility, Is.EqualTo(0.6f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.EqualTo(1.4f).Within(0.1f));
    }

    [Test]
    public void WorldContext_PostRainMorning_FullVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.PostRain,
            Season = Season.Spring
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // PostRain should have normal visibility (good for worm-hunting birds)
        Assert.That(context.Visibility, Is.EqualTo(1.0f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.EqualTo(1.0f).Within(0.1f));
    }

    [Test]
    public void WorldContext_OvercastDusk_ModeratelyLowVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dusk,
            Weather = Weather.Overcast,
            Season = Season.Fall
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Dusk = 0.5f, Overcast = 0.85f, combined = 0.425f
        Assert.That(context.Visibility, Is.EqualTo(0.425f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.GreaterThan(1.5f));
    }

    [Test]
    public void WorldContext_DifficultyMultiplierNeverLessThanOne()
    {
        // Test various conditions to ensure multiplier is always >= 1.0
        var conditions = new[]
        {
            (TimeOfDay.Morning, Weather.Clear),
            (TimeOfDay.Afternoon, Weather.PartlyCloudy),
            (TimeOfDay.Midday, Weather.PostRain)
        };

        foreach (var (time, weather) in conditions)
        {
            var context = new WorldContext
            {
                TimeOfDay = time,
                Weather = weather,
                Season = Season.Summer
            };

            context.RecalculateFactors();

            Assert.That(context.DifficultyMultiplier, Is.GreaterThanOrEqualTo(1.0f),
                $"Multiplier should be >= 1.0 for {time} and {weather}");
        }
    }

    [Test]
    public void WorldContext_SerializesToJson()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.PartlyCloudy,
            Season = Season.Summer,
            Visibility = 0.9f,
            DifficultyMultiplier = 1.1f,
            CurrentDate = new DateTime(2026, 6, 15, 14, 30, 0)
        };

        // Act
        string json = JsonConvert.SerializeObject(context);
        var deserialized = JsonConvert.DeserializeObject<WorldContext>(json);

        // Assert
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized.TimeOfDay, Is.EqualTo(context.TimeOfDay));
        Assert.That(deserialized.Weather, Is.EqualTo(context.Weather));
        Assert.That(deserialized.Season, Is.EqualTo(context.Season));
        Assert.That(deserialized.Visibility, Is.EqualTo(context.Visibility).Within(0.01f));
        Assert.That(deserialized.DifficultyMultiplier, Is.EqualTo(context.DifficultyMultiplier).Within(0.01f));
        Assert.That(deserialized.CurrentDate, Is.EqualTo(context.CurrentDate));
    }

    [Test]
    public void WorldContext_SerializesWithNullDate()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Dawn,
            Weather = Weather.Fog,
            Season = Season.Winter,
            CurrentDate = null
        };

        // Act
        string json = JsonConvert.SerializeObject(context);
        var deserialized = JsonConvert.DeserializeObject<WorldContext>(json);

        // Assert
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized.TimeOfDay, Is.EqualTo(context.TimeOfDay));
        Assert.That(deserialized.Weather, Is.EqualTo(context.Weather));
        Assert.That(deserialized.Season, Is.EqualTo(context.Season));
        Assert.That(deserialized.CurrentDate, Is.Null);
    }

    [Test]
    public void WorldContext_AllTimeOfDayValues_ProduceValidVisibility()
    {
        // Test all time of day values
        foreach (TimeOfDay timeOfDay in Enum.GetValues(typeof(TimeOfDay)))
        {
            var context = new WorldContext
            {
                TimeOfDay = timeOfDay,
                Weather = Weather.Clear,
                Season = Season.Spring
            };

            context.RecalculateFactors();

            Assert.That(context.Visibility, Is.GreaterThanOrEqualTo(0.0f),
                $"Visibility should be >= 0.0 for {timeOfDay}");
            Assert.That(context.Visibility, Is.LessThanOrEqualTo(1.0f),
                $"Visibility should be <= 1.0 for {timeOfDay}");
        }
    }

    [Test]
    public void WorldContext_AllWeatherValues_ProduceValidVisibility()
    {
        // Test all weather values
        foreach (Weather weather in Enum.GetValues(typeof(Weather)))
        {
            var context = new WorldContext
            {
                TimeOfDay = TimeOfDay.Morning,
                Weather = weather,
                Season = Season.Spring
            };

            context.RecalculateFactors();

            Assert.That(context.Visibility, Is.GreaterThanOrEqualTo(0.0f),
                $"Visibility should be >= 0.0 for {weather}");
            Assert.That(context.Visibility, Is.LessThanOrEqualTo(1.0f),
                $"Visibility should be <= 1.0 for {weather}");
        }
    }

    [Test]
    public void WorldContext_WorstConditions_LowestVisibility()
    {
        // Arrange - Worst possible conditions: Night + Fog
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Night,
            Weather = Weather.Fog,
            Season = Season.Winter
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Night = 0.2f, Fog = 0.3f, combined = 0.06f
        Assert.That(context.Visibility, Is.EqualTo(0.06f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.GreaterThan(1.9f));
    }

    [Test]
    public void WorldContext_WindyConditions_NormalVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Afternoon,
            Weather = Weather.Windy,
            Season = Season.Fall
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Windy should not affect visibility (birds behave differently but visibility is normal)
        Assert.That(context.Visibility, Is.EqualTo(0.9f).Within(0.01f));
    }

    [Test]
    public void WorldContext_LightRainMidday_ModerateVisibility()
    {
        // Arrange
        var context = new WorldContext
        {
            TimeOfDay = TimeOfDay.Midday,
            Weather = Weather.LightRain,
            Season = Season.Summer
        };

        // Act
        context.RecalculateFactors();

        // Assert
        // Midday = 1.0f, LightRain = 0.7f, combined = 0.7f
        Assert.That(context.Visibility, Is.EqualTo(0.7f).Within(0.01f));
        Assert.That(context.DifficultyMultiplier, Is.EqualTo(1.3f).Within(0.1f));
    }
}
