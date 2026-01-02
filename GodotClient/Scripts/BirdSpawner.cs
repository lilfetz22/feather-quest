using Godot;
using FeatherQuest.Core.Services;
using FeatherQuest.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatherQuest.Godot.Scripts;

/// <summary>
/// BirdSpawner node that uses Core logic to determine when and what to spawn.
/// This is the "Connector" or "Presenter" logic bridging Core data to Godot nodes.
/// </summary>
public partial class BirdSpawner : Node
{
    private BirdLoader _birdLoader;
    private IReadOnlyDictionary<string, BirdDefinition> _birdDatabase;
    private Timer _spawnTimer;
    private Random _random;
    
    // Configurable spawn parameters
    [Export]
    public float MinSpawnInterval { get; set; } = 3.0f;
    
    [Export]
    public float MaxSpawnInterval { get; set; } = 8.0f;
    
    [Export]
    public Vector2 MinSpawnPosition { get; set; } = new Vector2(0, 0);
    
    [Export]
    public Vector2 MaxSpawnPosition { get; set; } = new Vector2(1024, 600);
    
    [Export]
    public string BirdDataJsonPath { get; set; } = "res://Data/birds.json";

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Loads bird data and initializes the spawn timer.
    /// </summary>
    public override void _Ready()
    {
        _random = new Random();
        _birdLoader = new BirdLoader();
        
        // Load bird data
        LoadBirdData();
        
        // Initialize spawn timer
        InitializeSpawnTimer();
        
        GD.Print($"BirdSpawner initialized with {_birdDatabase?.Count ?? 0} birds loaded.");
    }

    /// <summary>
    /// Loads bird data from JSON file using BirdLoader from Core.
    /// </summary>
    private void LoadBirdData()
    {
        try
        {
            if (FileAccess.FileExists(BirdDataJsonPath))
            {
                using var file = FileAccess.Open(BirdDataJsonPath, FileAccess.ModeFlags.Read);
                string jsonContent = file.GetAsText();
                _birdDatabase = _birdLoader.LoadFromJson(jsonContent);
            }
            else
            {
                GD.PrintErr($"Bird data file not found at {BirdDataJsonPath}");
                // Create empty dictionary to prevent null reference errors
                _birdDatabase = new Dictionary<string, BirdDefinition>();
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to load bird data: {ex.Message}");
            _birdDatabase = new Dictionary<string, BirdDefinition>();
        }
    }

    /// <summary>
    /// Initializes the spawn timer with random intervals.
    /// </summary>
    private void InitializeSpawnTimer()
    {
        _spawnTimer = new Timer();
        AddChild(_spawnTimer);
        _spawnTimer.OneShot = false;
        _spawnTimer.WaitTime = GetRandomSpawnInterval();
        _spawnTimer.Timeout += OnSpawnTimerTimeout;
        _spawnTimer.Start();
    }

    /// <summary>
    /// Called when the spawn timer times out. Triggers a bird spawn.
    /// </summary>
    private void OnSpawnTimerTimeout()
    {
        SpawnBird();
        
        // Set next spawn interval
        _spawnTimer.WaitTime = GetRandomSpawnInterval();
    }

    /// <summary>
    /// Spawns a bird by selecting a random bird definition and plumage variant.
    /// </summary>
    private void SpawnBird()
    {
        if (_birdDatabase == null || _birdDatabase.Count == 0)
        {
            GD.PrintErr("Cannot spawn bird: No birds loaded in database");
            return;
        }

        // Select random bird definition
        var birdDefinition = SelectRandomBird();
        if (birdDefinition == null)
        {
            GD.PrintErr("Failed to select random bird");
            return;
        }

        // Select random plumage variant
        var plumageVariant = SelectRandomPlumageVariant(birdDefinition);
        if (plumageVariant == null)
        {
            GD.PrintErr($"Failed to select plumage variant for {birdDefinition.CommonName}");
            return;
        }

        // Generate random spawn position
        var spawnPosition = GetRandomSpawnPosition();

        // Instantiate bird cue (for now, just log - actual scene instantiation will be added later)
        GD.Print($"Spawning {birdDefinition.CommonName} ({plumageVariant.PlumageType} {plumageVariant.Gender}) at position {spawnPosition}");
        
        // TODO: When BirdCue.tscn is created, instantiate it here
        // var birdCueScene = GD.Load<PackedScene>("res://Scenes/BirdCue.tscn");
        // var birdCueInstance = birdCueScene.Instantiate();
        // birdCueInstance.Position = spawnPosition;
        // AddChild(birdCueInstance);
        
        // Optional: Play bird call if available
        if (birdDefinition.Calls.Count > 0)
        {
            var call = birdDefinition.Calls[_random.Next(birdDefinition.Calls.Count)];
            GD.Print($"  - Call type: {call.Type}, Audio: {call.AudioPath.Path}");
        }
    }

    /// <summary>
    /// Selects a random bird from the database.
    /// </summary>
    /// <returns>A random BirdDefinition, or null if database is empty.</returns>
    public BirdDefinition SelectRandomBird()
    {
        if (_birdDatabase == null || _birdDatabase.Count == 0)
        {
            return null;
        }

        var birdList = _birdDatabase.Values.ToList();
        int index = _random.Next(birdList.Count);
        return birdList[index];
    }

    /// <summary>
    /// Selects a random plumage variant from a bird definition.
    /// </summary>
    /// <param name="birdDefinition">The bird definition to select a variant from.</param>
    /// <returns>A random PlumageVariant, or null if no variants exist.</returns>
    public PlumageVariant SelectRandomPlumageVariant(BirdDefinition birdDefinition)
    {
        if (birdDefinition == null || birdDefinition.Variants.Count == 0)
        {
            return null;
        }

        int index = _random.Next(birdDefinition.Variants.Count);
        return birdDefinition.Variants[index];
    }

    /// <summary>
    /// Gets a random spawn interval between MinSpawnInterval and MaxSpawnInterval.
    /// </summary>
    /// <returns>A random float value representing seconds.</returns>
    private float GetRandomSpawnInterval()
    {
        return (float)(_random.NextDouble() * (MaxSpawnInterval - MinSpawnInterval) + MinSpawnInterval);
    }

    /// <summary>
    /// Gets a random spawn position within the configured bounds.
    /// </summary>
    /// <returns>A Vector2 representing the spawn position.</returns>
    public Vector2 GetRandomSpawnPosition()
    {
        float x = (float)(_random.NextDouble() * (MaxSpawnPosition.X - MinSpawnPosition.X) + MinSpawnPosition.X);
        float y = (float)(_random.NextDouble() * (MaxSpawnPosition.Y - MinSpawnPosition.Y) + MinSpawnPosition.Y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Gets the currently loaded bird database.
    /// </summary>
    /// <returns>Read-only dictionary of bird definitions.</returns>
    public IReadOnlyDictionary<string, BirdDefinition> GetBirdDatabase()
    {
        return _birdDatabase;
    }

    /// <summary>
    /// Allows injecting a bird database for testing purposes.
    /// </summary>
    /// <param name="birdDatabase">The bird database to use.</param>
    public void SetBirdDatabase(IReadOnlyDictionary<string, BirdDefinition> birdDatabase)
    {
        _birdDatabase = birdDatabase;
    }
}
