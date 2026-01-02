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
		
		// Create the binocular mask (black overlay with circular cutouts)
		var mask = new TextureRect
		{
			Name = "BinocularMask",
			Texture = GD.Load<Texture2D>("res://Assets/Textures/binocular_mask.svg")
		};
		// Set mask to cover entire screen
		mask.AnchorsPreset = (int)Control.LayoutPreset.FullRect;
		mask.GrowHorizontal = Control.GrowDirection.Both;
		mask.GrowVertical = Control.GrowDirection.Both;
		binocularView.AddChild(mask);
		
		// Create the bird container (moves with sway)
		var birdContainer = new Control
		{
			Name = "BirdContainer"
		};
		birdContainer.AnchorsPreset = (int)Control.LayoutPreset.Center;
		birdContainer.Size = new Vector2(400, 400);
		birdContainer.Position = new Vector2(0, 0);
		binocularView.AddChild(birdContainer);
		
		// Create the bird sprite
		var birdSprite = new TextureRect
		{
			Name = "BirdSprite",
			Texture = GD.Load<Texture2D>("res://Assets/Textures/placeholder_bird.svg")
		};
		birdSprite.AnchorsPreset = (int)Control.LayoutPreset.Center;
		birdSprite.Size = new Vector2(200, 200);
		birdSprite.Position = new Vector2(-100, -100); // Center it relative to parent
		birdContainer.AddChild(birdSprite);
		
		// Create the reticle container (stays centered)
		var reticle = new Control
		{
			Name = "Reticle"
		};
		reticle.AnchorsPreset = (int)Control.LayoutPreset.Center;
		reticle.Size = new Vector2(100, 100);
		reticle.Position = new Vector2(0, 0);
		binocularView.AddChild(reticle);
		
		// Create the reticle sprite
		var reticleSprite = new TextureRect
		{
			Name = "ReticleSprite",
			Texture = GD.Load<Texture2D>("res://Assets/Textures/reticle.svg")
		};
		reticleSprite.AnchorsPreset = (int)Control.LayoutPreset.Center;
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
