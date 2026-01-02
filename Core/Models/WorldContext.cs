using System;

namespace FeatherQuest.Core.Models;

/// <summary>
/// Represents the current environmental conditions in the game world.
/// Used by spawning systems, difficulty scaling, and AI behavior.
/// </summary>
public class WorldContext
{
    /// <summary>
    /// Current time of day in the game world.
    /// </summary>
    public TimeOfDay TimeOfDay { get; set; }

    /// <summary>
    /// Current weather conditions.
    /// </summary>
    public Weather Weather { get; set; }

    /// <summary>
    /// Current season (affects plumage variants and migration).
    /// </summary>
    public Season Season { get; set; }

    /// <summary>
    /// Normalized visibility factor (0.0 = zero visibility, 1.0 = perfect visibility).
    /// Calculated based on TimeOfDay and Weather.
    /// </summary>
    public float Visibility { get; set; }

    /// <summary>
    /// Risk/reward multiplier for current conditions.
    /// Low visibility = higher multiplier = more rewards.
    /// </summary>
    public float DifficultyMultiplier { get; set; }

    /// <summary>
    /// Optional: In-game date/time for calendar-based events.
    /// Can be nullable or default to DateTime.MinValue if not yet implemented.
    /// </summary>
    public DateTime? CurrentDate { get; set; }

    /// <summary>
    /// Calculates visibility and difficulty multiplier based on current conditions.
    /// Call this after changing TimeOfDay or Weather.
    /// </summary>
    public void RecalculateFactors()
    {
        // Base visibility by time of day
        float timeVisibility = TimeOfDay switch
        {
            TimeOfDay.Dawn => 0.6f,
            TimeOfDay.Morning => 1.0f,
            TimeOfDay.Midday => 1.0f,
            TimeOfDay.Afternoon => 0.9f,
            TimeOfDay.Dusk => 0.5f,
            TimeOfDay.Night => 0.2f,
            _ => 1.0f
        };

        // Weather modifier
        float weatherVisibility = Weather switch
        {
            Weather.Fog => 0.3f,
            Weather.HeavyRain => 0.5f,
            Weather.LightRain => 0.7f,
            Weather.Snow => 0.6f,
            Weather.Overcast => 0.85f,
            Weather.PartlyCloudy => 1.0f,
            Weather.Clear => 1.0f,
            Weather.PostRain => 1.0f,
            Weather.Windy => 1.0f,
            _ => 1.0f
        };

        // Combined visibility (multiplicative)
        Visibility = timeVisibility * weatherVisibility;

        // Difficulty multiplier (inverse of visibility, clamped)
        // Low visibility = high difficulty = high reward
        DifficultyMultiplier = Math.Max(1.0f, 2.0f - Visibility);
    }
}
