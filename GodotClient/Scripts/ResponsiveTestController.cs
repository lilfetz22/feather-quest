using Godot;
using FeatherQuest.GodotClient.Scripts;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Test controller for responsive layout and input testing.
/// Press 'B' to toggle binocular view.
/// Press 'I' to display viewport info.
/// </summary>
public partial class ResponsiveTestController : Node
{
	[Export] public NodePath BinocularViewPath { get; set; } = new NodePath("BinocularView");
	[Export] public NodePath InfoLabelPath { get; set; } = new NodePath("DebugOverlay/InfoLabel");
	
	private BinocularView _binocularView;
	private Label _infoLabel;
	
	public override void _Ready()
	{
		// Find the BinocularView
		_binocularView = GetNodeOrNull<BinocularView>(BinocularViewPath);
		
		// Find the info label
		_infoLabel = GetNodeOrNull<Label>(InfoLabelPath);
		
		if (_binocularView == null)
		{
			GD.PrintErr($"BinocularView not found at path: {BinocularViewPath}");
		}
		
		// Update info on start
		UpdateInfo();
		
		GD.Print("=== Responsive Test Controller Ready ===");
		GD.Print("Press 'B' to toggle Binocular View");
		GD.Print("Press 'I' to refresh viewport info");
		GD.Print("Press 'F' to toggle fullscreen");
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho())
		{
			switch (keyEvent.Keycode)
			{
				case Key.B:
					// Toggle binoculars
					_binocularView?.Toggle();
					break;
					
				case Key.I:
					// Refresh info
					UpdateInfo();
					break;
					
				case Key.F:
					// Toggle fullscreen
					ToggleFullscreen();
					break;
			}
		}
	}
	
	private void UpdateInfo()
	{
		var window = GetWindow();
		if (window == null)
		{
			GD.PrintErr("Unable to get window reference");
			return;
		}
		
		var size = window.Size;
		var aspectRatio = (float)size.X / size.Y;
		var aspectString = GetAspectRatioString(aspectRatio);
		
		var info = $"Responsive Layout Test\n" +
		           $"Resolution: {size.X}x{size.Y}\n" +
		           $"Aspect Ratio: {aspectRatio:F2} ({aspectString})\n" +
		           $"Stretch Mode: canvas_items\n" +
		           $"Aspect: expand\n" +
		           $"\n" +
		           $"Controls:\n" +
		           $"Press 'B' to toggle Binocular View\n" +
		           $"Press 'I' to refresh info\n" +
		           $"Press 'F' to toggle fullscreen";
		
		if (_infoLabel != null)
		{
			_infoLabel.Text = info;
		}
		
		GD.Print("=== Viewport Info ===");
		GD.Print($"Resolution: {size.X}x{size.Y}");
		GD.Print($"Aspect Ratio: {aspectRatio:F2} ({aspectString})");
	}
	
	private string GetAspectRatioString(float ratio)
	{
		// Common aspect ratios
		if (Mathf.Abs(ratio - 16f / 9f) < 0.01f) return "16:9";
		if (Mathf.Abs(ratio - 9f / 16f) < 0.01f) return "9:16 Portrait";
		if (Mathf.Abs(ratio - 4f / 3f) < 0.01f) return "4:3";
		if (Mathf.Abs(ratio - 3f / 4f) < 0.01f) return "3:4 Portrait";
		if (Mathf.Abs(ratio - 21f / 9f) < 0.01f) return "21:9 Ultrawide";
		if (Mathf.Abs(ratio - 32f / 9f) < 0.01f) return "32:9 Super Ultrawide";
		if (Mathf.Abs(ratio - 19.5f / 9f) < 0.1f) return "~19.5:9 (iPhone X)";
		
		return $"{ratio:F2}:1";
	}
	
	private void ToggleFullscreen()
	{
		var window = GetWindow();
		if (window == null) return;
		
		var currentMode = window.Mode;
		if (currentMode == Window.ModeEnum.Fullscreen)
		{
			window.Mode = Window.ModeEnum.Windowed;
			GD.Print("Switched to Windowed mode");
		}
		else
		{
			window.Mode = Window.ModeEnum.Fullscreen;
			GD.Print("Switched to Fullscreen mode");
		}
		
		// Update info after mode change
		CallDeferred(nameof(UpdateInfo));
	}
}
