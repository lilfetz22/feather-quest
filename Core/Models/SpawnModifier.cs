namespace FeatherQuest.Core.Models;

/// <summary>
/// Contains spawn behavior modifications based on environmental conditions.
/// Returned by SpawnRuleEngine to influence spawner behavior.
/// </summary>
public class SpawnModifier
{
    /// <summary>
    /// The spawn pattern to use (Solitary, Flock, Swarm, Hidden).
    /// </summary>
    public SpawnPattern Pattern { get; set; }

    /// <summary>
    /// Spawn rate multiplier (1.0 = normal, 0.5 = half rate, 2.0 = double rate).
    /// </summary>
    public float SpawnRateMultiplier { get; set; }

    /// <summary>
    /// Number of birds to spawn in a group (1 for Solitary, 2-4 for Flock, 5+ for Swarm).
    /// </summary>
    public int GroupSize { get; set; }

    /// <summary>
    /// Creates a default spawn modifier with normal behavior.
    /// </summary>
    public SpawnModifier()
    {
        Pattern = SpawnPattern.Solitary;
        SpawnRateMultiplier = 1.0f;
        GroupSize = 1;
    }
}
