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

Unit tests for the BirdSpawner logic are available in `Tests/BirdSpawnerLogicTests.cs`. Run tests with:

```bash
dotnet test --filter "FullyQualifiedName~BirdSpawnerLogicTests"
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
