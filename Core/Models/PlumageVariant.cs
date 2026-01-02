namespace FeatherQuest.Core.Models;

public class PlumageVariant
{
    public PlumageType PlumageType { get; set; }
    public Gender Gender { get; set; }
    public AssetReference SpritePath { get; set; } = new AssetReference();
    public DifficultyTier DifficultyRating { get; set; }
}
