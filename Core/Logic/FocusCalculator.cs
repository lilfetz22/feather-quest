using FeatherQuest.Core.Models;
using System;

namespace FeatherQuest.Core.Logic;

/// <summary>
/// Pure math calculator for binocular focus, sway mechanics, and photo quality scoring.
/// All methods are stateless and engine-agnostic (no Godot or Unity dependencies).
/// </summary>
public static class FocusCalculator
{
    /// <summary>
    /// Maximum possible distance from center to corner (sqrt(2)).
    /// </summary>
    private const float MaxDistance = 1.41421356f;

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

    /// <summary>
    /// Calculates the quality of a bird photograph based on reticle positioning and stability.
    /// Quality is determined by the formula: quality = Clamp01((1 - max(0, normalizedDistance - centerTolerance)) * stabilityAvg),
    /// where normalizedDistance is the distance from center normalized to the 0-1 range and centerTolerance reduces the
    /// effective distance (values within the tolerance are treated as perfect, and the adjustment is clamped at 0).
    /// </summary>
    /// <param name="finalReticlePos">Final reticle position when photo was taken (normalized coordinates, center = 0,0)</param>
    /// <param name="stabilityAvg">Average stability during the encounter (0.0 = completely unstable, 1.0 = perfectly stable)</param>
    /// <param name="centerTolerance">Distance from center considered "perfect" (default: 0.0, meaning exact center only). Valid range: 0.0 to 1.0.</param>
    /// <returns>Quality score from 0.0 (worst) to 1.0 (perfect)</returns>
    public static float CalculatePhotoQuality(Vector2Simple finalReticlePos, float stabilityAvg, float centerTolerance = 0.0f)
    {
        // Clamp stability to valid range [0, 1]
        stabilityAvg = Clamp01(stabilityAvg);

        // Clamp tolerance to valid range [0, 1]
        centerTolerance = Clamp01(centerTolerance);

        // Calculate Euclidean distance from center (0, 0)
        float distance = finalReticlePos.Magnitude();

        // Normalize distance to 0-1 range (0 = center, 1 = corner)
        float normalizedDistance = distance / MaxDistance;

        // Apply tolerance (allows slight off-center to still be "perfect")
        float adjustedDistance = Math.Max(0, normalizedDistance - centerTolerance);

        // Calculate quality (inverse of distance, scaled by stability)
        // If stability is 0, quality is 0 regardless of centering
        float quality = (1.0f - adjustedDistance) * stabilityAvg;

        // Ensure quality is clamped to [0, 1]
        return Clamp01(quality);
    }

    /// <summary>
    /// Clamps a float value to the range [0, 1].
    /// </summary>
    private static float Clamp01(float value)
    {
        if (value < 0f) return 0f;
        if (value > 1f) return 1f;
        return value;
    }
}
