using Godot;
using FeatherQuest.Core.Logic;
using FeatherQuest.Core.Models;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Example integration showing how to use BinocularView in a complete encounter flow.
/// This demonstrates the full lifecycle: Start -> Track Focus -> End -> Score Photo
/// </summary>
public partial class EncounterIntegrationExample : Node
{
	private BinocularView _binocularView;
	private float _focusAccumulator = 0f;
	private float _focusRequirement = 100f;
	private float _stabilityTracker = 0f;
	private int _stabilityMeasurements = 0;
	
	public override void _Ready()
	{
		_binocularView = GetNode<BinocularView>("BinocularView");
		
		// Example: Start an encounter when clicking on a bird cue
		// In real game, this would be triggered by the spawn system
	}
	
	/// <summary>
	/// Called when player clicks on a bird cue in exploration mode.
	/// </summary>
	public void OnBirdCueClicked(Texture2D birdTexture)
	{
		// Start the binocular encounter
		_binocularView.StartEncounter(birdTexture);
		_focusAccumulator = 0f;
		_stabilityTracker = 0f;
		_stabilityMeasurements = 0;
	}
	
	public override void _Process(double delta)
	{
		// Only track if binocular view is active
		if (!_binocularView.Visible)
			return;
		
		// Get the bird's current offset from center
		Vector2Simple offset = _binocularView.GetBirdOffsetFromCenter();
		
		// Calculate how well-centered the bird is (0 = perfect, higher = worse)
		float distanceFromCenter = offset.Magnitude();
		float clampedDistanceFromCenter = Mathf.Clamp(distanceFromCenter, 0f, 1f);
		
		// Award focus based on centering
		// The closer to center, the faster focus builds
		float focusGain = (1.0f - clampedDistanceFromCenter) * (float)delta * 50f;
		_focusAccumulator += Mathf.Max(0, focusGain);
		
		// Track stability for photo quality calculation
		// Lower offset = higher stability contribution
		float currentStability = 1.0f - clampedDistanceFromCenter;
		_stabilityTracker += currentStability;
		_stabilityMeasurements++;
		
		// Check if focus is complete
		if (_focusAccumulator >= _focusRequirement)
		{
			CompleteFocus();
		}
		
		// Optional: Display focus meter in UI
		UpdateFocusMeterUI(_focusAccumulator / _focusRequirement);
	}
	
	private void CompleteFocus()
	{
		// Calculate average stability during the encounter
		float averageStability = _stabilityMeasurements > 0 
			? _stabilityTracker / _stabilityMeasurements 
			: 0f;
		
		// Get final position for photo quality
		Vector2Simple finalPosition = _binocularView.GetBirdOffsetFromCenter();
		
		// Calculate photo quality using Core logic
		float photoQuality = FocusCalculator.CalculatePhotoQuality(
			finalPosition, 
			averageStability,
			centerTolerance: 0.1f // Allow slight off-center
		);
		
		// Determine medal tier
		string medal = DeterminePhotoMedal(photoQuality);
		
		// End the encounter
		_binocularView.EndEncounter();
		
		// Show results to player
		ShowEncounterResults(photoQuality, medal);
		
		// Transition to identification phase
		TransitionToIdentification();
	}
	
	private string DeterminePhotoMedal(float quality)
	{
		// Based on the project spec:
		// Gold: High quality, well-centered and stable
		// Silver: Medium quality
		// Bronze: Low quality but acceptable
		
		if (quality >= 0.85f)
			return "Gold";
		else if (quality >= 0.60f)
			return "Silver";
		else if (quality >= 0.35f)
			return "Bronze";
		else
			return "None"; // Failed to get a good shot
	}
	
	private void UpdateFocusMeterUI(float percentage)
	{
		// TODO: Update UI element showing focus progress
		// Example: ProgressBar node showing 0-100%
		GD.Print($"Focus: {percentage * 100:F1}%");
	}
	
	private void ShowEncounterResults(float quality, string medal)
	{
		GD.Print("=== Encounter Complete ===");
		GD.Print($"Photo Quality: {quality:F2}");
		GD.Print($"Medal: {medal}");
		GD.Print("========================");
		
		// TODO: Show result screen with photo quality and medal
	}
	
	private void TransitionToIdentification()
	{
		// TODO: Load identification UI where player selects bird features
		GD.Print("Transitioning to Identification phase...");
	}
	
	/// <summary>
	/// Optional: Allow player to cancel the encounter (bird flushes).
	/// </summary>
	public void OnBirdFlushed()
	{
		_binocularView.EndEncounter();
		GD.Print("Bird flushed! Encounter failed.");
	}
	
	/// <summary>
	/// Handle input for canceling encounter (ESC key).
	/// </summary>
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho())
		{
			if (keyEvent.Keycode == Key.Escape && _binocularView.Visible)
			{
				OnBirdFlushed();
			}
		}
	}
}
