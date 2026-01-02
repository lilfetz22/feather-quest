namespace FeatherQuest.Core.Models;

/// <summary>
/// A simple 2D vector struct for engine-agnostic math operations.
/// Does not depend on Godot or Unity vector types.
/// </summary>
public readonly struct Vector2Simple
{
    /// <summary>
    /// X component of the vector.
    /// </summary>
    public float X { get; }

    /// <summary>
    /// Y component of the vector.
    /// </summary>
    public float Y { get; }

    /// <summary>
    /// Creates a new Vector2Simple with specified X and Y components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    public Vector2Simple(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Calculates the Euclidean distance from the origin (0, 0).
    /// </summary>
    /// <returns>The magnitude (length) of this vector.</returns>
    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }
}
