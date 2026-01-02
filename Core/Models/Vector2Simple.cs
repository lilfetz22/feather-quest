namespace FeatherQuest.Core.Models;

/// <summary>
/// A simple 2D vector struct for engine-agnostic math operations.
/// Does not depend on Godot or Unity vector types.
/// </summary>
public struct Vector2Simple
{
    /// <summary>
    /// X component of the vector.
    /// </summary>
    public float X;

    /// <summary>
    /// Y component of the vector.
    /// </summary>
    public float Y;

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
}
