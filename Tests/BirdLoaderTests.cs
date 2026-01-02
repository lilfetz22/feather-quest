using FeatherQuest.Core.Services;
using FeatherQuest.Core.Models;
using System.Text.Json;

namespace FeatherQuest.Tests;

public class BirdLoaderTests
{
    [Test]
    public void LoadBirdDatabase_ReturnsCorrectVariants()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        loader.LoadFromJson(json);
        var robin = loader.Get("robin");
        Assert.That(robin, Is.Not.Null);
        Assert.That(robin.CommonName, Is.EqualTo("American Robin"));
        Assert.That(robin.Variants.Count, Is.GreaterThan(0));
        Assert.That(robin.Calls.Count, Is.GreaterThan(0));
    }

    [Test]
    public void LoadBirdDatabase_ParsesAllFields()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        loader.LoadFromJson(json);
        var robin = loader.Get("robin");
        
        Assert.That(robin.ID, Is.EqualTo("robin"));
        Assert.That(robin.CommonName, Is.EqualTo("American Robin"));
        Assert.That(robin.ScientificName, Is.EqualTo("Turdus migratorius"));
        Assert.That(robin.Tier, Is.EqualTo(DifficultyTier.Beginner));
        Assert.That(robin.FieldMarks.Length, Is.EqualTo(3));
        Assert.That(robin.FieldMarks, Does.Contain("orange_breast"));
        Assert.That(robin.FieldMarks, Does.Contain("gray_back"));
        Assert.That(robin.FieldMarks, Does.Contain("yellow_bill"));
    }

    [Test]
    public void LoadBirdDatabase_ParsesVariants()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        loader.LoadFromJson(json);
        var robin = loader.Get("robin");
        
        Assert.That(robin.Variants.Count, Is.EqualTo(1));
        var variant = robin.Variants[0];
        Assert.That(variant.PlumageType, Is.EqualTo(PlumageType.Breeding));
        Assert.That(variant.Gender, Is.EqualTo(Gender.Male));
        Assert.That(variant.SpritePath.Path, Is.EqualTo("res://Assets/Birds/robin_breeding_male.png"));
        Assert.That(variant.DifficultyRating, Is.EqualTo(DifficultyTier.Beginner));
    }

    [Test]
    public void LoadBirdDatabase_ParsesCalls()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        loader.LoadFromJson(json);
        var robin = loader.Get("robin");
        
        Assert.That(robin.Calls.Count, Is.EqualTo(1));
        var call = robin.Calls[0];
        Assert.That(call.Type, Is.EqualTo(CallType.Song));
        Assert.That(call.AudioPath.Path, Is.EqualTo("res://Assets/Audio/robin_song.mp3"));
        Assert.That(call.SpectrogramPath.Path, Is.EqualTo("res://Assets/Spectrograms/robin_song.png"));
    }

    [Test]
    public void Get_ThrowsKeyNotFoundException_WhenBirdNotFound()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        loader.LoadFromJson(json);
        
        Assert.Throws<KeyNotFoundException>(() => loader.Get("nonexistent"));
    }

    [Test]
    public void LoadFromJson_ReturnsDictionary()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        var db = loader.LoadFromJson(json);
        
        Assert.That(db, Is.Not.Null);
        Assert.That(db.Count, Is.EqualTo(1));
        Assert.That(db.ContainsKey("robin"), Is.True);
    }

    [Test]
    public void LoadFromJson_ThrowsArgumentNullException_WhenJsonIsNull()
    {
        var loader = new BirdLoader();
        Assert.Throws<ArgumentNullException>(() => loader.LoadFromJson(null!));
    }

    [Test]
    public void LoadFromJson_ThrowsArgumentNullException_WhenJsonIsEmpty()
    {
        var loader = new BirdLoader();
        Assert.Throws<ArgumentNullException>(() => loader.LoadFromJson(string.Empty));
    }

    [Test]
    public void LoadFromJson_ThrowsJsonException_WhenJsonIsInvalid()
    {
        var loader = new BirdLoader();
        var invalidJson = "{ this is not valid json }";
        Assert.Throws<JsonException>(() => loader.LoadFromJson(invalidJson));
    }

    [Test]
    public void LoadFromJson_ThrowsInvalidOperationException_WhenBirdsArrayIsMissing()
    {
        var loader = new BirdLoader();
        var jsonWithoutBirds = "{ \"notBirds\": [] }";
        var ex = Assert.Throws<InvalidOperationException>(() => loader.LoadFromJson(jsonWithoutBirds));
        Assert.That(ex.Message, Does.Contain("Failed to parse bird data from JSON"));
    }

    [Test]
    public void LoadFromJson_ReturnsReadOnlyDictionary()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        var db = loader.LoadFromJson(json);
        
        Assert.That(db, Is.InstanceOf<IReadOnlyDictionary<string, BirdDefinition>>());
    }

    [Test]
    public void LoadFromJson_ReturnedDictionaryDoesNotAffectInternalState()
    {
        var json = File.ReadAllText("TestData/birds.json");
        var loader = new BirdLoader();
        var db = loader.LoadFromJson(json);
        
        // Verify we can still get the bird
        var robin = loader.Get("robin");
        Assert.That(robin, Is.Not.Null);
        
        // Try to modify the returned dictionary (if it's actually mutable)
        if (db is Dictionary<string, BirdDefinition> mutableDict)
        {
            mutableDict.Clear();
        }
        
        // Internal state should be unaffected - we should still be able to get the bird
        var robinAfter = loader.Get("robin");
        Assert.That(robinAfter, Is.Not.Null);
        Assert.That(robinAfter.CommonName, Is.EqualTo("American Robin"));
    }
}
