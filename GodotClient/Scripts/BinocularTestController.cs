using Godot;
using FeatherQuest.GodotClient.Scripts;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Test controller for the Binocular UI.
/// Press 'B' to toggle the binocular view on/off.
/// </summary>
public partial class BinocularTestController : Node
{
	[Export] public NodePath BinocularViewPath { get; set; } = new NodePath("BinocularView");
	
	private BinocularView _binocularView;
	
	public override void _Ready()
	{
		// Find the BinocularView in the scene using the exported path
		_binocularView = GetNodeOrNull<BinocularView>(BinocularViewPath);
		
		if (_binocularView == null)
		{
			GD.PrintErr($"BinocularView not found at path: {BinocularViewPath}. Press 'B' will not work.");
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho() && keyEvent.Keycode == Key.B)
		{
			// Press 'B' to toggle binoculars
			_binocularView?.Toggle();
		}
	}
}
