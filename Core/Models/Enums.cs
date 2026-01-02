namespace FeatherQuest.Core.Models;

/// <summary>
/// Plumage variant type based on breeding cycle and age.
/// </summary>
public enum PlumageType
{
    Breeding,
    NonBreeding,
    Molting,
    Juvenile
}

public enum Gender
{
    Male,
    Female,
    Unknown
}

public enum CallType
{
    Song,
    Call,
    Alarm,
    Flight
}

public enum DifficultyTier
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}
