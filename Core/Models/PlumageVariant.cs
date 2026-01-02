namespace FeatherQuest.Core.Models;

using System.Text.Json.Serialization;

public class PlumageVariant
{
    [JsonPropertyName("season")]
    public PlumageType PlumageType { get; set; }
    public Gender Gender { get; set; }
    public AssetReference SpritePath { get; set; } = new AssetReference();
    public DifficultyTier DifficultyRating { get; set; }
}
