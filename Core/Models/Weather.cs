namespace FeatherQuest.Core.Models;

/// <summary>
/// Represents weather conditions that affect bird behavior and visibility.
/// </summary>
public enum Weather
{
    /// <summary>
    /// Normal visibility and spawn rates.
    /// </summary>
    Clear,

    /// <summary>
    /// No major effects on visibility or behavior.
    /// </summary>
    PartlyCloudy,

    /// <summary>
    /// Slightly reduced visibility conditions.
    /// </summary>
    Overcast,

    /// <summary>
    /// Waterfowl active, most birds sheltering.
    /// </summary>
    LightRain,

    /// <summary>
    /// Most birds hidden, challenge increased significantly.
    /// </summary>
    HeavyRain,

    /// <summary>
    /// Worms emerge, robins and thrushes swarm.
    /// </summary>
    PostRain,

    /// <summary>
    /// Severely reduced visibility, high risk/reward conditions.
    /// </summary>
    Fog,

    /// <summary>
    /// Limited species available, visibility reduced.
    /// </summary>
    Snow,

    /// <summary>
    /// Birds hunkered down, flight behavior erratic.
    /// </summary>
    Windy
}
