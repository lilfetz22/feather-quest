using Godot;
using FeatherQuest.Core.Services;
using FeatherQuest.Core.Models;
using FeatherQuest.Core.Logic;
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
    private SpawnRuleEngine _spawnRuleEngine;
    private WorldContext _worldContext;
    
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
        _spawnRuleEngine = new SpawnRuleEngine();
        
        // Initialize default world context (clear morning)
        _worldContext = new WorldContext
        {
            TimeOfDay = TimeOfDay.Morning,
            Weather = Weather.Clear,
            Season = Season.Spring
        };
        _worldContext.RecalculateFactors();
        
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

    public override void _ExitTree()
    {
        CleanupSpawnTimer();
        base._ExitTree();
    }

    /// <summary>
    /// Cleans up the spawn timer to avoid leaks and callbacks after this node leaves the tree.
    /// </summary>
    private void CleanupSpawnTimer()
    {
        if (_spawnTimer == null)
        {
            return;
        }

        _spawnTimer.Stop();
        _spawnTimer.Timeout -= OnSpawnTimerTimeout;

        if (_spawnTimer.GetParent() == this)
        {
            RemoveChild(_spawnTimer);
        }

        _spawnTimer.QueueFree();
        _spawnTimer = null;
    }

    /// <summary>
    /// Called when the spawn timer times out. Triggers a bird spawn.
    /// </summary>
    private void OnSpawnTimerTimeout()
    {
        // Evaluate spawn rules based on current world context
        var spawnModifier = _spawnRuleEngine.EvaluateSpawnRules(_worldContext);
        
        // Apply spawn pattern - spawn appropriate number of birds
        SpawnBirdGroup(spawnModifier);
        
        // Set next spawn interval adjusted by spawn rate multiplier
        const float MinSpawnRateMultiplier = 0.2f;
        var effectiveMultiplier = spawnModifier.SpawnRateMultiplier;

        if (effectiveMultiplier < MinSpawnRateMultiplier)
        {
            effectiveMultiplier = MinSpawnRateMultiplier;
        }

        _spawnTimer.WaitTime = GetRandomSpawnInterval() / effectiveMultiplier;
    }

    /// <summary>
    /// Spawns a group of birds based on the spawn modifier pattern.
    /// </summary>
    /// <param name="modifier">The spawn modifier containing pattern and group size.</param>
    private void SpawnBirdGroup(SpawnModifier modifier)
    {
        if (_birdDatabase == null || _birdDatabase.Count == 0)
        {
            GD.PrintErr("Cannot spawn bird: No birds loaded in database");
            return;
        }

        // Don't spawn at all if pattern is Hidden and rate is very low
        if (modifier.Pattern == SpawnPattern.Hidden && _random.NextDouble() > modifier.SpawnRateMultiplier)
        {
            GD.Print($"Birds are hiding (Pattern: {modifier.Pattern})");
            return;
        }

        // Spawn the appropriate number of birds based on group size
        for (int i = 0; i < modifier.GroupSize; i++)
        {
            SpawnBird(modifier.Pattern, i, modifier.GroupSize);
        }
    }

    /// <summary>
    /// Spawns a single bird by selecting a random bird definition and plumage variant.
    /// </summary>
    /// <param name="pattern">The spawn pattern being used.</param>
    /// <param name="indexInGroup">Index of this bird in its group (0 for solo spawns).</param>
    /// <param name="groupSize">Total size of the group being spawned.</param>
    private void SpawnBird(SpawnPattern pattern = SpawnPattern.Solitary, int indexInGroup = 0, int groupSize = 1)
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

        // Generate random spawn position (with offset for grouped spawns)
        var spawnPosition = GetRandomSpawnPosition();
        
        // If spawning in a group, offset positions to cluster them
        if (groupSize > 1)
        {
            float offsetX = (indexInGroup - groupSize / 2.0f) * 50; // 50 pixels apart
            float offsetY = (float)(_random.NextDouble() * 40 - 20); // Random vertical variance
            spawnPosition.X += offsetX;
            spawnPosition.Y += offsetY;
        }

        // Instantiate bird cue (for now, just log - actual scene instantiation will be added later)
        string patternInfo = groupSize > 1 ? $"[{pattern} {indexInGroup + 1}/{groupSize}]" : $"[{pattern}]";
        GD.Print($"Spawning {birdDefinition.CommonName} ({plumageVariant.PlumageType} {plumageVariant.Gender}) {patternInfo} at position {spawnPosition}");
        
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
            if (!string.IsNullOrEmpty(call.SpectrogramPath.Path))
            {
                GD.Print($"  - Spectrogram: {call.SpectrogramPath.Path}");
            }
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

    /// <summary>
    /// Gets the current world context.
    /// </summary>
    /// <returns>The current world context.</returns>
    public WorldContext GetWorldContext()
    {
        return _worldContext;
    }

    /// <summary>
    /// Sets the world context to influence spawn behavior.
    /// </summary>
    /// <param name="context">The world context to use.</param>
    public void SetWorldContext(WorldContext context)
    {
        _worldContext = context;
        _worldContext.RecalculateFactors();
        GD.Print($"World context updated: {context.TimeOfDay}, {context.Weather}, {context.Season}");
    }
}
