using System.Text.Json;
using FeatherQuest.Core.Models;

namespace FeatherQuest.Core.Services;

public class BirdLoader
{
    private Dictionary<string, BirdDefinition> _birdDatabase = new Dictionary<string, BirdDefinition>();

    public Dictionary<string, BirdDefinition> LoadFromJson(string jsonContent)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        var birdData = JsonSerializer.Deserialize<BirdData>(jsonContent, options);
        
        if (birdData?.Birds == null)
        {
            throw new InvalidOperationException("Failed to parse bird data from JSON");
        }

        _birdDatabase.Clear();
        foreach (var bird in birdData.Birds)
        {
            _birdDatabase[bird.ID] = bird;
        }

        return _birdDatabase;
    }

    public BirdDefinition Get(string birdId)
    {
        if (_birdDatabase.TryGetValue(birdId, out var bird))
        {
            return bird;
        }
        throw new KeyNotFoundException($"Bird with ID '{birdId}' not found in database");
    }

    private class BirdData
    {
        public List<BirdDefinition> Birds { get; set; } = new List<BirdDefinition>();
    }
}
