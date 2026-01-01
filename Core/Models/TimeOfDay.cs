namespace FeatherQuest.Core.Models;

/// <summary>
/// Represents different times of day that affect bird activity and visibility.
/// </summary>
public enum TimeOfDay
{
    /// <summary>
    /// 5:00-7:00 AM - High bird activity, warblers active.
    /// </summary>
    Dawn,

    /// <summary>
    /// 7:00-11:00 AM - Peak birding time with highest activity.
    /// </summary>
    Morning,

    /// <summary>
    /// 11:00 AM-2:00 PM - Reduced activity, raptors hunting.
    /// </summary>
    Midday,

    /// <summary>
    /// 2:00-5:00 PM - Moderate activity levels.
    /// </summary>
    Afternoon,

    /// <summary>
    /// 5:00-7:00 PM - Second activity peak, low visibility conditions.
    /// </summary>
    Dusk,

    /// <summary>
    /// 7:00 PM-5:00 AM - Owls and nightjars only, very low visibility.
    /// </summary>
    Night
}
