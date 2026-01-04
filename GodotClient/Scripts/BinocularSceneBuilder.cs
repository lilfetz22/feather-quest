using Godot;
using FeatherQuest.GodotClient.Scripts;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Example scene builder that demonstrates how to create the Binocular UI programmatically.
/// This can be used as a reference or as a starter template.
/// In production, you would typically build this scene in the Godot editor instead.
/// </summary>
public partial class BinocularSceneBuilder : Node
{
	public override void _Ready()
	{
		BuildBinocularScene();
	}
	
	private void BuildBinocularScene()
	{
		// Create the BinocularView CanvasLayer
		var binocularView = new BinocularView
		{
			Name = "BinocularView",
			Layer = 100,
			Stability = 0.5f,
			SwayAmplitude = 0.2f,
			MouseSensitivity = 1.0f,
			ViewportSize = 400f
		};
		AddChild(binocularView);
		
		// Load textures with null checks
		var maskTexture = GD.Load<Texture2D>("res://Assets/Textures/binocular_mask.svg");
		var birdTexture = GD.Load<Texture2D>("res://Assets/Textures/placeholder_bird.svg");
		var reticleTexture = GD.Load<Texture2D>("res://Assets/Textures/reticle.svg");
		
		if (maskTexture == null)
		{
			GD.PrintErr("Failed to load binocular mask texture. Check if the file exists at res://Assets/Textures/binocular_mask.svg");
		}
		if (birdTexture == null)
		{
			GD.PrintErr("Failed to load bird texture. Check if the file exists at res://Assets/Textures/placeholder_bird.svg");
		}
		if (reticleTexture == null)
		{
			GD.PrintErr("Failed to load reticle texture. Check if the file exists at res://Assets/Textures/reticle.svg");
		}
		
		// Create the binocular mask (black overlay with circular cutouts)
		var mask = new TextureRect
		{
			Name = "BinocularMask",
			Texture = maskTexture
		};
		// Set mask to cover entire screen
		mask.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		mask.GrowHorizontal = Control.GrowDirection.Both;
		mask.GrowVertical = Control.GrowDirection.Both;
		binocularView.AddChild(mask);
		
		// Create the bird container (moves with sway)
		var birdContainer = new Control
		{
			Name = "BirdContainer"
		};
		birdContainer.SetAnchorsPreset(Control.LayoutPreset.Center);
		birdContainer.Size = new Vector2(400, 400);
		birdContainer.Position = new Vector2(0, 0);
		binocularView.AddChild(birdContainer);
		
		// Create the bird sprite
		var birdSprite = new TextureRect
		{
			Name = "BirdSprite",
			Texture = birdTexture
		};
		birdSprite.SetAnchorsPreset(Control.LayoutPreset.Center);
		birdSprite.Size = new Vector2(200, 200);
		birdSprite.Position = new Vector2(-100, -100); // Center it relative to parent
		birdContainer.AddChild(birdSprite);
		
		// Create the reticle container (stays centered)
		var reticle = new Control
		{
			Name = "Reticle"
		};
		reticle.SetAnchorsPreset(Control.LayoutPreset.Center);
		reticle.Size = new Vector2(100, 100);
		reticle.Position = new Vector2(0, 0);
		binocularView.AddChild(reticle);
		
		// Create the reticle sprite
		var reticleSprite = new TextureRect
		{
			Name = "ReticleSprite",
			Texture = reticleTexture
		};
		reticleSprite.SetAnchorsPreset(Control.LayoutPreset.Center);
		reticleSprite.Size = new Vector2(100, 100);
		reticleSprite.Position = new Vector2(-50, -50); // Center it relative to parent
		reticle.AddChild(reticleSprite);
		
		// Add test controller
		var testController = new BinocularTestController
		{
			Name = "BinocularTestController"
		};
		AddChild(testController);
		
		GD.Print("Binocular scene built successfully!");
		GD.Print("Press 'B' to toggle the binocular view.");
	}
}
