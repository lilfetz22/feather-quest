using System.Text.Json;
using FeatherQuest.Core.Models;

namespace FeatherQuest.Core.Services;

public class BirdLoader
{
    private readonly Dictionary<string, BirdDefinition> _birdDatabase = new Dictionary<string, BirdDefinition>();

    /// <summary>
    /// Parses JSON content and loads bird definitions into the database.
    /// This method is not thread-safe and should be called from a single thread.
    /// </summary>
    /// <param name="jsonContent">The JSON string containing bird definitions.</param>
    /// <returns>A read-only dictionary of bird definitions keyed by bird ID.</returns>
    /// <exception cref="ArgumentNullException">Thrown when jsonContent is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when JSON parsing fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON is malformed.</exception>
    public IReadOnlyDictionary<string, BirdDefinition> LoadFromJson(string jsonContent)
    {
        if (string.IsNullOrEmpty(jsonContent))
        {
            throw new ArgumentNullException(nameof(jsonContent), "JSON content cannot be null or empty");
        }

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

        return new Dictionary<string, BirdDefinition>(_birdDatabase);
    }

    /// <summary>
    /// Retrieves a bird definition by its ID.
    /// </summary>
    /// <param name="birdId">The unique identifier of the bird.</param>
    /// <returns>The bird definition matching the specified ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no bird with the specified ID exists in the database.</exception>
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
        public List<BirdDefinition>? Birds { get; set; }
    }
}
