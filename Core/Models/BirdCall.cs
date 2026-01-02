namespace FeatherQuest.Core.Models;

public class BirdCall
{
    public CallType Type { get; set; }
    public AssetReference AudioPath { get; set; } = new AssetReference();
    public AssetReference SpectrogramPath { get; set; } = new AssetReference();
}
