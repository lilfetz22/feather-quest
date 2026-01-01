using FeatherQuest.Core.Logic;
using FeatherQuest.Core.Models;
using System;

namespace FeatherQuest.Tests;

/// <summary>
/// Unit tests for the FocusCalculator class.
/// Tests validate sway behavior under different stability conditions.
/// </summary>
public class FocusCalculatorTests
{
    [Test]
    public void CalculateSway_FullStability_ReturnsMinimalSway()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float stability = 1.0f;

        // Act
        var sway = FocusCalculator.CalculateSway(elapsedTime, stability);

        // Assert - with full stability (1.0), sway should be zero or near-zero
        Assert.That(Math.Abs(sway.X), Is.LessThan(0.01f), "X sway should be minimal with full stability");
        Assert.That(Math.Abs(sway.Y), Is.LessThan(0.01f), "Y sway should be minimal with full stability");
    }

    [Test]
    public void CalculateSway_NoStability_ReturnsMaximumSway()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float stability = 0.0f;
        float swayAmplitude = 0.2f;

        // Act
        var sway = FocusCalculator.CalculateSway(elapsedTime, stability, swayAmplitude);

        // Assert - with no stability (0.0), sway should be significant
        // At time=1.0, sin(1.5) ≈ 0.997 and sin(2.0) ≈ 0.909, so we expect values near amplitude
        Assert.That(Math.Abs(sway.X), Is.GreaterThan(0.1f), "X sway should be significant with no stability");
        Assert.That(Math.Abs(sway.Y), Is.GreaterThan(0.1f), "Y sway should be significant with no stability");
    }

    [Test]
    public void CalculateSway_SameInputs_ReturnsSameOutput()
    {
        // Arrange
        float elapsedTime = 5.0f;
        float stability = 0.5f;

        // Act
        var sway1 = FocusCalculator.CalculateSway(elapsedTime, stability);
        var sway2 = FocusCalculator.CalculateSway(elapsedTime, stability);

        // Assert - deterministic behavior: same inputs should produce same outputs
        Assert.That(sway1.X, Is.EqualTo(sway2.X), "X sway should be deterministic");
        Assert.That(sway1.Y, Is.EqualTo(sway2.Y), "Y sway should be deterministic");
    }

    [Test]
    public void CalculateSway_DifferentTimes_ProducesDifferentSway()
    {
        // Arrange
        float stability = 0.5f;

        // Act
        var sway1 = FocusCalculator.CalculateSway(0.0f, stability);
        var sway2 = FocusCalculator.CalculateSway(1.0f, stability);

        // Assert - sway should change over time
        Assert.That(sway1.X, Is.Not.EqualTo(sway2.X), "X sway should change over time");
        Assert.That(sway1.Y, Is.Not.EqualTo(sway2.Y), "Y sway should change over time");
    }

    [Test]
    public void CalculateSway_MediumStability_ProducesModerateSwayComparison()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float lowStability = 0.2f;
        float highStability = 0.8f;

        // Act
        var lowStabilitySway = FocusCalculator.CalculateSway(elapsedTime, lowStability);
        var highStabilitySway = FocusCalculator.CalculateSway(elapsedTime, highStability);

        // Assert - lower stability should produce more sway than higher stability
        Assert.That(Math.Abs(lowStabilitySway.X), Is.GreaterThan(Math.Abs(highStabilitySway.X)),
            "Lower stability should produce more X sway");
        Assert.That(Math.Abs(lowStabilitySway.Y), Is.GreaterThan(Math.Abs(highStabilitySway.Y)),
            "Lower stability should produce more Y sway");
    }

    [Test]
    public void CalculateSway_XAndYAxesIndependent()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float stability = 0.5f;

        // Act
        var sway = FocusCalculator.CalculateSway(elapsedTime, stability);

        // Assert - X and Y should have different values due to different frequencies
        Assert.That(sway.X, Is.Not.EqualTo(sway.Y), "X and Y sway should be independent (different frequencies)");
    }

    [Test]
    public void CalculateSway_CustomAmplitude_AffectsSwayMagnitude()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float stability = 0.0f;
        float smallAmplitude = 0.1f;
        float largeAmplitude = 0.5f;

        // Act
        var smallSway = FocusCalculator.CalculateSway(elapsedTime, stability, smallAmplitude);
        var largeSway = FocusCalculator.CalculateSway(elapsedTime, stability, largeAmplitude);

        // Assert - larger amplitude should produce larger sway
        Assert.That(Math.Abs(largeSway.X), Is.GreaterThan(Math.Abs(smallSway.X)),
            "Larger amplitude should produce more X sway");
        Assert.That(Math.Abs(largeSway.Y), Is.GreaterThan(Math.Abs(smallSway.Y)),
            "Larger amplitude should produce more Y sway");
    }

    [Test]
    public void CalculateSway_StabilityBelowZero_ClampedToZero()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float invalidStability = -0.5f;
        float validStability = 0.0f;

        // Act
        var invalidSway = FocusCalculator.CalculateSway(elapsedTime, invalidStability);
        var validSway = FocusCalculator.CalculateSway(elapsedTime, validStability);

        // Assert - negative stability should be clamped to 0.0, producing same result as 0.0 stability
        Assert.That(invalidSway.X, Is.EqualTo(validSway.X), "Negative stability should be clamped to 0.0 for X");
        Assert.That(invalidSway.Y, Is.EqualTo(validSway.Y), "Negative stability should be clamped to 0.0 for Y");
    }

    [Test]
    public void CalculateSway_StabilityAboveOne_ClampedToOne()
    {
        // Arrange
        float elapsedTime = 1.0f;
        float invalidStability = 1.5f;
        float validStability = 1.0f;

        // Act
        var invalidSway = FocusCalculator.CalculateSway(elapsedTime, invalidStability);
        var validSway = FocusCalculator.CalculateSway(elapsedTime, validStability);

        // Assert - stability above 1.0 should be clamped to 1.0, producing same result as 1.0 stability
        Assert.That(invalidSway.X, Is.EqualTo(validSway.X), "Stability above 1.0 should be clamped for X");
        Assert.That(invalidSway.Y, Is.EqualTo(validSway.Y), "Stability above 1.0 should be clamped for Y");
    }

    [Test]
    public void CalculateSway_ZeroTime_ReturnsZeroSway()
    {
        // Arrange
        float elapsedTime = 0.0f;
        float stability = 0.0f;

        // Act
        var sway = FocusCalculator.CalculateSway(elapsedTime, stability);

        // Assert - at time zero, sin(0) = 0, so sway should be zero regardless of stability
        Assert.That(Math.Abs(sway.X), Is.LessThan(0.0001f), "X sway should be zero at time=0");
        Assert.That(Math.Abs(sway.Y), Is.LessThan(0.0001f), "Y sway should be zero at time=0");
    }

    [Test]
    public void CalculateSway_SmoothContinuousMovement()
    {
        // Arrange
        float stability = 0.5f;
        float deltaTime = 0.1f;

        // Act - sample sway at multiple consecutive time points
        var sway1 = FocusCalculator.CalculateSway(0.0f, stability);
        var sway2 = FocusCalculator.CalculateSway(deltaTime, stability);
        var sway3 = FocusCalculator.CalculateSway(deltaTime * 2, stability);

        // Assert - sway should change smoothly (no sudden jumps)
        // The change between consecutive samples should be relatively small
        float deltaX12 = Math.Abs(sway2.X - sway1.X);
        float deltaX23 = Math.Abs(sway3.X - sway2.X);
        float deltaY12 = Math.Abs(sway2.Y - sway1.Y);
        float deltaY23 = Math.Abs(sway3.Y - sway2.Y);

        Assert.That(deltaX12, Is.LessThan(0.2f), "X sway change should be smooth");
        Assert.That(deltaX23, Is.LessThan(0.2f), "X sway change should be smooth");
        Assert.That(deltaY12, Is.LessThan(0.2f), "Y sway change should be smooth");
        Assert.That(deltaY23, Is.LessThan(0.2f), "Y sway change should be smooth");
    }
}
