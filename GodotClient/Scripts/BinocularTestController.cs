using Godot;
using FeatherQuest.GodotClient.Scripts;

namespace FeatherQuest.GodotClient.Scripts;

/// <summary>
/// Test controller for the Binocular UI.
/// Press 'B' to toggle the binocular view on/off.
/// </summary>
public partial class BinocularTestController : Node
{
	private BinocularView _binocularView;
	
	public override void _Ready()
	{
		// Find the BinocularView in the scene
		_binocularView = GetNode<BinocularView>("/root/Main/BinocularView");
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
