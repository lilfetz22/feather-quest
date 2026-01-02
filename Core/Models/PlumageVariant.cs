namespace FeatherQuest.Core.Models;

public class PlumageVariant
{
    public Season Season { get; set; }
    public Gender Gender { get; set; }
    public AssetReference SpritePath { get; set; } = new AssetReference();
    public DifficultyTier DifficultyRating { get; set; }
}
