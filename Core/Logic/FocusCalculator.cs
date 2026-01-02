using FeatherQuest.Core.Models;

namespace FeatherQuest.Core.Logic;

/// <summary>
/// Provides calculation methods for focus and photo quality mechanics.
/// </summary>
public static class FocusCalculator
{
    /// <summary>
    /// Maximum possible distance from center to corner (sqrt(2)).
    /// </summary>
    private const float MaxDistance = 1.41421356f;

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
