namespace FeatherQuest.Core.Models;

/// <summary>
/// Represents seasonal periods that affect plumage variants and migration patterns.
/// </summary>
public enum Season
{
    /// <summary>
    /// Migration peak, breeding plumage visible.
    /// </summary>
    Spring,

    /// <summary>
    /// Breeding season, juvenile birds appear.
    /// </summary>
    Summer,

    /// <summary>
    /// Fall migration, molting and non-breeding plumage.
    /// </summary>
    Fall,

    /// <summary>
    /// Reduced species diversity, winter plumage visible.
    /// </summary>
    Winter
}
