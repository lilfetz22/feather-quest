using FeatherQuest.Core.Logic;
using FeatherQuest.Core.Models;

namespace FeatherQuest.Tests;

/// <summary>
/// Unit tests for the photo quality calculation system.
/// </summary>
public class PhotoQualityTests
{
    [Test]
    public void CalculatePhotoQuality_CenteredAndStable_ReturnsOne()
    {
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0, 0),
            stabilityAvg: 1.0f
        );
        Assert.That(quality, Is.EqualTo(1.0f).Within(0.01f));
    }

    [Test]
    public void CalculatePhotoQuality_CenteredButUnstable_ReturnsZero()
    {
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0, 0),
            stabilityAvg: 0.0f
        );
        Assert.That(quality, Is.EqualTo(0.0f).Within(0.01f));
    }

    [Test]
    public void CalculatePhotoQuality_OffCenterAndStable_ReturnsDegradedScore()
    {
        // Position at (0.5, 0.5) = approximately 50% from center
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.5f, 0.5f),
            stabilityAvg: 1.0f
        );
        // Should be less than 1.0 but greater than 0.0
        Assert.That(quality, Is.GreaterThan(0.0f));
        Assert.That(quality, Is.LessThan(1.0f));
    }

    [Test]
    public void CalculatePhotoQuality_CornerPosition_ReturnsLowScore()
    {
        // Position at corner (1, 1)
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(1.0f, 1.0f),
            stabilityAvg: 1.0f
        );
        Assert.That(quality, Is.LessThan(0.4f)); // Should be Bronze tier at best
    }

    [Test]
    public void CalculatePhotoQuality_WithTolerance_AllowsSlightlyOffCenter()
    {
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.1f, 0.1f),
            stabilityAvg: 1.0f,
            centerTolerance: 0.15f
        );
        // With tolerance, this should still be near-perfect
        Assert.That(quality, Is.GreaterThan(0.9f));
    }

    [Test]
    public void CalculatePhotoQuality_HalfStability_ReturnsHalfQuality()
    {
        // Centered shot but only 50% stability
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0, 0),
            stabilityAvg: 0.5f
        );
        Assert.That(quality, Is.EqualTo(0.5f).Within(0.01f));
    }

    [Test]
    public void CalculatePhotoQuality_InvalidStability_ClampedToValidRange()
    {
        // Negative stability should be clamped to 0
        var qualityNegative = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0, 0),
            stabilityAvg: -0.5f
        );
        Assert.That(qualityNegative, Is.EqualTo(0.0f).Within(0.01f));

        // Stability > 1 should be clamped to 1
        var qualityOverOne = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0, 0),
            stabilityAvg: 1.5f
        );
        Assert.That(qualityOverOne, Is.EqualTo(1.0f).Within(0.01f));
    }

    [Test]
    public void CalculatePhotoQuality_DegradationIsMonotonic()
    {
        // As we move away from center, quality should decrease monotonically
        var quality1 = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.0f, 0.0f),
            stabilityAvg: 1.0f
        );
        var quality2 = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.3f, 0.0f),
            stabilityAvg: 1.0f
        );
        var quality3 = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.6f, 0.0f),
            stabilityAvg: 1.0f
        );
        var quality4 = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(1.0f, 0.0f),
            stabilityAvg: 1.0f
        );

        Assert.That(quality1, Is.GreaterThan(quality2));
        Assert.That(quality2, Is.GreaterThan(quality3));
        Assert.That(quality3, Is.GreaterThan(quality4));
    }

    [Test]
    public void CalculatePhotoQuality_EdgePosition_HandlesGracefully()
    {
        // Position at edge (1, 0)
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(1.0f, 0.0f),
            stabilityAvg: 1.0f
        );
        // Should return a valid score, not crash
        Assert.That(quality, Is.GreaterThanOrEqualTo(0.0f));
        Assert.That(quality, Is.LessThanOrEqualTo(1.0f));
    }

    [Test]
    public void CalculatePhotoQuality_BronzeTierThreshold()
    {
        // Test scores that should fall in Bronze tier (0.0 - 0.4)
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.9f, 0.9f),
            stabilityAvg: 1.0f
        );
        Assert.That(quality, Is.LessThan(0.4f));
    }

    [Test]
    public void CalculatePhotoQuality_SilverTierThreshold()
    {
        // Test scores that should fall in Silver tier (0.4 - 0.7)
        // Moderate distance with perfect stability
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.3f, 0.3f),
            stabilityAvg: 1.0f
        );
        Assert.That(quality, Is.GreaterThanOrEqualTo(0.4f));
        Assert.That(quality, Is.LessThanOrEqualTo(0.7f));
    }

    [Test]
    public void CalculatePhotoQuality_GoldTierThreshold()
    {
        // Test scores that should fall in Gold tier (0.7 - 1.0)
        // Close to center with perfect stability
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.1f, 0.1f),
            stabilityAvg: 1.0f
        );
        Assert.That(quality, Is.GreaterThanOrEqualTo(0.7f));
    }

    [Test]
    public void CalculatePhotoQuality_CombinedFactors_RealisticScenario()
    {
        // Slightly off-center with good but not perfect stability
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.2f, 0.15f),
            stabilityAvg: 0.85f
        );
        
        // Should be a decent score but not perfect
        Assert.That(quality, Is.GreaterThan(0.5f));
        Assert.That(quality, Is.LessThan(0.95f));
    }

    [Test]
    public void CalculatePhotoQuality_NeverExceedsOne()
    {
        // Even with tolerance and perfect positioning, should never exceed 1.0
        var quality = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.0f, 0.0f),
            stabilityAvg: 1.0f,
            centerTolerance: 0.5f
        );
        Assert.That(quality, Is.LessThanOrEqualTo(1.0f));
    }

    [Test]
    public void CalculatePhotoQuality_ToleranceAffectsScore()
    {
        // Same position, different tolerance values
        var qualityNoTolerance = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.15f, 0.15f),
            stabilityAvg: 1.0f,
            centerTolerance: 0.0f
        );
        
        var qualityWithTolerance = FocusCalculator.CalculatePhotoQuality(
            new Vector2Simple(0.15f, 0.15f),
            stabilityAvg: 1.0f,
            centerTolerance: 0.2f
        );

        // With tolerance, the score should be better
        Assert.That(qualityWithTolerance, Is.GreaterThan(qualityNoTolerance));
    }
}
