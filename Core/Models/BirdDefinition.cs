namespace FeatherQuest.Core.Models;

public class BirdDefinition
{
    public string ID { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public DifficultyTier Tier { get; set; }
    public string[] FieldMarks { get; set; } = Array.Empty<string>();
    public List<PlumageVariant> Variants { get; set; } = new List<PlumageVariant>();
    public List<BirdCall> Calls { get; set; } = new List<BirdCall>();
}
