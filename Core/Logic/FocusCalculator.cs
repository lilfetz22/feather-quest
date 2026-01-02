using FeatherQuest.Core.Models;
using System;

namespace FeatherQuest.Core.Logic;

/// <summary>
/// Pure math calculator for binocular focus and sway mechanics.
/// All methods are stateless and engine-agnostic (no Godot or Unity dependencies).
/// </summary>
public static class FocusCalculator
{
    /// <summary>
    /// Calculates the binocular sway offset at a given time.
    /// Uses sine wave oscillation with different frequencies for X and Y axes
    /// to create realistic, non-circular hand shake motion.
    /// </summary>
    /// <param name="elapsedTime">Time in seconds since encounter started.</param>
    /// <param name="stability">Stability factor (0.0 = unstable/maximum sway, 1.0 = perfectly stable/no sway).</param>
    /// <param name="swayAmplitude">Maximum sway distance in normalized units (default: 0.2).</param>
    /// <returns>X/Y offset to apply to the reticle position.</returns>
    public static Vector2Simple CalculateSway(float elapsedTime, float stability, float swayAmplitude = 0.2f)
    {
        // Clamp stability to valid range [0.0, 1.0]
        stability = Math.Clamp(stability, 0.0f, 1.0f);

        // Inverse stability: more instability = more sway
        float instability = 1.0f - stability;

        // Different frequencies for X and Y to avoid circular patterns
        // These frequencies create a more organic, realistic hand shake
        float freqX = 1.5f;
        float freqY = 2.0f;

        // Calculate sway using sine waves with time-based phase
        // The instability factor scales the amplitude
        float offsetX = (float)Math.Sin(elapsedTime * freqX) * swayAmplitude * instability;
        float offsetY = (float)Math.Sin(elapsedTime * freqY) * swayAmplitude * instability;

        return new Vector2Simple(offsetX, offsetY);
    }
}
