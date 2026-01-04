namespace FeatherQuest.Core.Models;

/// <summary>
/// Defines behavioral patterns for bird spawning.
/// Used by spawn systems to determine how birds appear in the world.
/// </summary>
public enum SpawnPattern
{
    /// <summary>
    /// Single bird spawns - typical default behavior.
    /// </summary>
    Solitary,

    /// <summary>
    /// Small group of 2-4 birds spawning together.
    /// </summary>
    Flock,

    /// <summary>
    /// Large group of 5+ birds spawning together (e.g., after rain).
    /// </summary>
    Swarm,

    /// <summary>
    /// Birds are hiding and rarely spawn.
    /// </summary>
    Hidden
}
