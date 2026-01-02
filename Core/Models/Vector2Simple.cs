namespace FeatherQuest.Core.Models;

/// <summary>
/// A simple 2D vector structure for engine-agnostic coordinate representation.
/// </summary>
public readonly struct Vector2Simple
{
    public float X { get; }
    public float Y { get; }

    public Vector2Simple(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Calculates the Euclidean distance from the origin (0, 0).
    /// </summary>
    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }
}
