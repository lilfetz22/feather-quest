using Godot;
using FeatherQuest.Core.Logic;
using FeatherQuest.Core.Models;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Controls the binocular UI layer and handles the focus mechanic.
/// This is the "Battle" phase where the player stabilizes the view to focus on a bird.
/// </summary>
public partial class BinocularView : CanvasLayer
{
	// UI Elements (assigned via Godot editor)
	private Control _birdContainer;
	private TextureRect _birdSprite;
	private Control _reticle;
	
	// State
	private bool _isActive = false;
	private float _elapsedTime = 0f;
	private Vector2 _mouseOffset = Vector2.Zero;
	
	// Configuration
	[Export] public float Stability { get; set; } = 0.5f;
	[Export] public float SwayAmplitude { get; set; } = 0.2f;
	[Export] public float MouseSensitivity { get; set; } = 1.0f;
	[Export] public float ViewportSize { get; set; } = 400f; // Size of the binocular viewing area
	
	public override void _Ready()
	{
		// Get references to child nodes
		_birdContainer = GetNode<Control>("BirdContainer");
		_birdSprite = GetNode<TextureRect>("BirdContainer/BirdSprite");
		_reticle = GetNode<Control>("Reticle");
		
		// Initially hidden
		Hide();
	}
	
	public override void _Process(double delta)
	{
		if (!_isActive)
			return;
		
		_elapsedTime += (float)delta;
		
		// Calculate sway using the Core logic
		Vector2Simple sway = FocusCalculator.CalculateSway(_elapsedTime, Stability, SwayAmplitude);
		
		// Convert Vector2Simple to Godot Vector2 and scale by viewport size
		Vector2 swayOffset = new Vector2(sway.X, sway.Y) * ViewportSize;
		
		// Calculate final position: Sway - MouseOffset
		// The bird moves with sway, but player can counteract with mouse movement
		Vector2 finalPosition = swayOffset - _mouseOffset;
		
		// Apply position to the bird container, if it has been initialized
		if (_birdContainer != null)
		{
			_birdContainer.Position = finalPosition;
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!_isActive)
			return;
		
		Vector2 inputDelta = Vector2.Zero;
		
		// Handle mouse motion
		if (@event is InputEventMouseMotion mouseMotion)
		{
			inputDelta = mouseMotion.Relative;
		}
		// Handle touch drag (for mobile/tablet)
		else if (@event is InputEventScreenDrag touchDrag)
		{
			inputDelta = touchDrag.Relative;
		}
		
		// Apply input delta if we have any
		if (inputDelta != Vector2.Zero)
		{
			// Input movement counteracts the sway
			// Moving input right should move the bird right (counteracting left sway)
			_mouseOffset += inputDelta * MouseSensitivity;
			
			// Clamp offset to prevent moving too far
			float maxOffset = ViewportSize * SwayAmplitude * 2f;
			_mouseOffset = _mouseOffset.Clamp(
				new Vector2(-maxOffset, -maxOffset),
				new Vector2(maxOffset, maxOffset)
			);
		}
	}
	
	/// <summary>
	/// Activates the binocular view for a bird encounter.
	/// </summary>
	public void StartEncounter(Texture2D birdTexture = null)
	{
		_isActive = true;
		_elapsedTime = 0f;
		_mouseOffset = Vector2.Zero;
		
		// Set bird sprite if provided
		if (birdTexture != null && _birdSprite != null)
		{
			_birdSprite.Texture = birdTexture;
		}
		
		Show();
	}
	
	/// <summary>
	/// Deactivates the binocular view.
	/// </summary>
	public void EndEncounter()
	{
		_isActive = false;
		Hide();
	}
	
	/// <summary>
	/// Gets the current offset of the bird from the center (for quality calculation).
	/// Returns a normalized Vector2Simple where (0,0) is perfectly centered.
	/// </summary>
	public Vector2Simple GetBirdOffsetFromCenter()
	{
		if (_birdContainer == null)
			return new Vector2Simple(0, 0);
		
		Vector2 offset = _birdContainer.Position;
		
		// Normalize by viewport size to get values roughly in -1 to 1 range
		float normalizedX = offset.X / ViewportSize;
		float normalizedY = offset.Y / ViewportSize;
		
		return new Vector2Simple(normalizedX, normalizedY);
	}
	
	/// <summary>
	/// Toggles the binocular view on/off (for testing).
	/// </summary>
	public void Toggle()
	{
		if (_isActive)
			EndEncounter();
		else
			StartEncounter();
	}
}
