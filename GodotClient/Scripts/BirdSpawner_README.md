# BirdSpawner Usage Guide

## Overview
The `BirdSpawner` is a Godot Node that integrates with the FeatherQuest Core library to spawn birds in the game world. It handles loading bird data, selecting random birds and plumage variants, and managing spawn timing and positions.

## Adding BirdSpawner to Your Scene

### Option 1: Add via Script
```csharp
// In your scene's main script
var spawner = new BirdSpawner();
AddChild(spawner);
```

### Option 2: Add via Godot Editor
1. Create a new Node in your scene
2. Attach the `BirdSpawner.cs` script to the node
3. Configure the exported properties in the Inspector

## Configuration

The BirdSpawner exposes several configurable properties:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MinSpawnInterval` | float | 3.0 | Minimum time between spawns (seconds) |
| `MaxSpawnInterval` | float | 8.0 | Maximum time between spawns (seconds) |
| `MinSpawnPosition` | Vector2 | (0, 0) | Minimum X/Y coordinates for spawn position |
| `MaxSpawnPosition` | Vector2 | (1024, 600) | Maximum X/Y coordinates for spawn position |
| `BirdDataJsonPath` | string | "res://Data/birds.json" | Path to the bird data JSON file |

### Example Configuration
```csharp
var spawner = new BirdSpawner
{
    MinSpawnInterval = 5.0f,
    MaxSpawnInterval = 15.0f,
    MinSpawnPosition = new Vector2(100, 100),
    MaxSpawnPosition = new Vector2(1920, 1080),
    BirdDataJsonPath = "res://Data/birds.json"
};
AddChild(spawner);
```

## Bird Data Format

The spawner expects a JSON file with the following structure:

```json
{
  "birds": [
    {
      "id": "robin",
      "commonName": "American Robin",
      "scientificName": "Turdus migratorius",
      "tier": "Beginner",
      "fieldMarks": ["orange_breast", "gray_back", "yellow_bill"],
      "variants": [
        {
          "season": "Breeding",
          "gender": "Male",
          "spritePath": {
            "path": "res://Assets/Birds/robin_breeding_male.png"
          },
          "difficultyRating": "Beginner"
        }
      ],
      "calls": [
        {
          "type": "Song",
          "audioPath": {
            "path": "res://Assets/Audio/robin_song.mp3"
          },
          "spectrogramPath": {
            "path": "res://Assets/Spectrograms/robin_song.png"
          }
        }
      ]
    }
  ]
}
```

## Integration with BirdCue Scenes

The BirdSpawner is designed to instantiate `BirdCue` scenes when they are available. Currently, it logs spawn events to the console. To enable scene instantiation:

1. Create a `BirdCue.tscn` scene in `res://Scenes/`
2. Uncomment the instantiation code in `BirdSpawner.cs`:

```csharp
// In the SpawnBird() method:
var birdCueScene = GD.Load<PackedScene>("res://Scenes/BirdCue.tscn");
var birdCueInstance = birdCueScene.Instantiate();
birdCueInstance.Position = spawnPosition;
AddChild(birdCueInstance);
```

3. Add logic to pass bird data to the BirdCue instance

## Features

### Dynamic Behavioral Spawning (NEW)
The spawner now adjusts bird behavior based on environmental conditions using the `SpawnRuleEngine`:

#### Weather-Based Behavior
- **PostRain**: Triggers swarm behavior (5 birds in groups) with 2x spawn rate - robins hunt for worms
- **HeavyRain**: Birds hide, spawn rate drops to 20%
- **LightRain**: Reduced spawning (50% rate), waterfowl more active
- **Fog**: Significantly reduced spawning (40% rate)
- **Snow**: Limited species availability (60% rate)
- **Windy**: Birds hunker down (70% rate)

#### Time-Based Behavior
- **Dawn/Morning**: Peak activity - flocks of 2-3 birds, 1.5x spawn rate
- **Midday**: Reduced activity - solitary birds, 50% spawn rate
- **Dusk**: Secondary activity peak - small flocks, 1.3x spawn rate
- **Night**: Nocturnal birds only - hidden pattern, 30% spawn rate

### Controlling World Context
You can dynamically change environmental conditions at runtime:

```csharp
// Get the spawner
var spawner = GetNode<BirdSpawner>("BirdSpawner");

// Create a post-rain morning scenario (optimal birding!)
var context = new WorldContext
{
    TimeOfDay = TimeOfDay.Morning,
    Weather = Weather.PostRain,
    Season = Season.Spring
};
spawner.SetWorldContext(context);

// Birds will now spawn in swarms with increased frequency
```

### Random Bird Selection
The spawner randomly selects birds from the loaded database, ensuring variety in spawns.

### Random Plumage Variant Selection
Each bird can have multiple plumage variants (different seasons, genders). The spawner randomly selects one variant per spawn.

### Configurable Spawn Timing
Spawn intervals are randomized within the configured min/max range, creating natural-feeling spawn patterns.

### Position Randomization
Spawn positions are randomized within the configured bounds, distributing birds across the gameplay area.

### Bird Call Support
The spawner tracks which bird calls are available and can be used to play audio cues when birds spawn.

## Architecture Notes

The BirdSpawner follows the MVP (Model-View-Presenter) pattern:
- **Model**: `BirdDefinition`, `PlumageVariant`, etc. in `FeatherQuest.Core.Models`
- **View**: Godot scenes (BirdCue.tscn, etc.)
- **Presenter**: `BirdSpawner.cs` - bridges Core data to Godot nodes

This design ensures the spawning logic is engine-agnostic and can be migrated to Unity or other engines in the future.

## Testing

Unit tests for the BirdSpawner logic and spawn rules are available in the Tests directory:
- `Tests/BirdSpawnerLogicTests.cs` - Core spawner selection and integration tests
- `Tests/SpawnRuleEngineTests.cs` - Behavioral spawning rules

Run all tests with:

```bash
dotnet test
```

Run specific test suites:

```bash
# Just spawner tests
dotnet test --filter "FullyQualifiedName~BirdSpawnerLogicTests"

# Just spawn rule tests
dotnet test --filter "FullyQualifiedName~SpawnRuleEngineTests"
```

### Testing Spawn Behaviors in Game
To verify spawn behaviors are working:

```csharp
// In a test scene or controller script:
var spawner = GetNode<BirdSpawner>("BirdSpawner");

// Test 1: PostRain swarm behavior
var postRain = new WorldContext { 
    TimeOfDay = TimeOfDay.Morning, 
    Weather = Weather.PostRain, 
    Season = Season.Spring 
};
spawner.SetWorldContext(postRain);
// Observe: Should spawn 5 birds at once, twice as frequently

// Test 2: Midday reduced activity
var midday = new WorldContext { 
    TimeOfDay = TimeOfDay.Midday, 
    Weather = Weather.Clear, 
    Season = Season.Summer 
};
spawner.SetWorldContext(midday);
// Observe: Should spawn single birds half as frequently

// Test 3: Heavy rain hiding
var heavyRain = new WorldContext { 
    TimeOfDay = TimeOfDay.Afternoon, 
    Weather = Weather.HeavyRain, 
    Season = Season.Fall 
};
spawner.SetWorldContext(heavyRain);
// Observe: Birds should rarely appear
```

## Troubleshooting

### "Bird data file not found" error
- Verify the `BirdDataJsonPath` property points to a valid JSON file
- Check that the file exists in the project
- Ensure the path uses the `res://` protocol

### No birds spawning
- Check the Godot console for error messages
- Verify the JSON file is properly formatted
- Ensure the spawner node is added to the scene tree
- Check that birds in the database have at least one variant

### Birds spawning outside visible area
- Adjust `MinSpawnPosition` and `MaxSpawnPosition` to match your gameplay area
- Consider the camera's viewport size when setting bounds
