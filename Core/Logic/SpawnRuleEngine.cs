using System;

namespace FeatherQuest.Core.Models;

/// <summary>
/// Evaluates WorldContext to determine spawn behavior modifications.
/// Pure logic class with no engine dependencies - testable without Godot.
/// </summary>
public class SpawnRuleEngine
{
    /// <summary>
    /// Evaluates the current world context and returns appropriate spawn modifications.
    /// </summary>
    /// <param name="context">Current world environmental conditions.</param>
    /// <returns>SpawnModifier containing pattern and rate adjustments.</returns>
    public SpawnModifier EvaluateSpawnRules(WorldContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modifier = new SpawnModifier();

        // Weather-based rules (highest priority)
        switch (context.Weather)
        {
            case Weather.PostRain:
                // Worms emerge, robins and thrushes swarm
                modifier.Pattern = SpawnPattern.Swarm;
                modifier.SpawnRateMultiplier = 2.0f;
                modifier.GroupSize = 5;
                return modifier;

            case Weather.HeavyRain:
                // Most birds hidden, challenge increased
                modifier.Pattern = SpawnPattern.Hidden;
                modifier.SpawnRateMultiplier = 0.2f;
                modifier.GroupSize = 1;
                return modifier;

            case Weather.LightRain:
                // Waterfowl active, most birds sheltering
                modifier.Pattern = SpawnPattern.Solitary;
                modifier.SpawnRateMultiplier = 0.5f;
                modifier.GroupSize = 1;
                return modifier;

            case Weather.Fog:
                // Severely reduced visibility, birds less active
                modifier.Pattern = SpawnPattern.Solitary;
                modifier.SpawnRateMultiplier = 0.4f;
                modifier.GroupSize = 1;
                return modifier;

            case Weather.Snow:
                // Limited species, reduced activity
                modifier.Pattern = SpawnPattern.Solitary;
                modifier.SpawnRateMultiplier = 0.6f;
                modifier.GroupSize = 1;
                return modifier;

            case Weather.Windy:
                // Birds hunkered down, erratic flight
                modifier.Pattern = SpawnPattern.Solitary;
                modifier.SpawnRateMultiplier = 0.7f;
                modifier.GroupSize = 1;
                return modifier;
        }

        // Time of day rules (applied if no weather override)
        switch (context.TimeOfDay)
        {
            case TimeOfDay.Dawn:
            case TimeOfDay.Morning:
                // Peak birding time, high activity
                modifier.Pattern = SpawnPattern.Flock;
                modifier.SpawnRateMultiplier = 1.5f;
                modifier.GroupSize = 3;
                return modifier;

            case TimeOfDay.Midday:
                // Reduced activity, raptors hunting
                modifier.Pattern = SpawnPattern.Solitary;
                modifier.SpawnRateMultiplier = 0.5f;
                modifier.GroupSize = 1;
                return modifier;

            case TimeOfDay.Dusk:
                // Second activity peak
                modifier.Pattern = SpawnPattern.Flock;
                modifier.SpawnRateMultiplier = 1.3f;
                modifier.GroupSize = 2;
                return modifier;

            case TimeOfDay.Night:
                // Owls and nightjars only
                modifier.Pattern = SpawnPattern.Hidden;
                modifier.SpawnRateMultiplier = 0.3f;
                modifier.GroupSize = 1;
                return modifier;
        }

        // Default: normal spawning
        modifier.Pattern = SpawnPattern.Solitary;
        modifier.SpawnRateMultiplier = 1.0f;
        modifier.GroupSize = 1;
        return modifier;
    }
}
