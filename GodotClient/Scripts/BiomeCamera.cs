using Godot;
using System;

/// <summary>
/// Controls camera scrolling for the biome scene.
/// Handles mouse drag and touch swipe input for horizontal scrolling.
/// </summary>
public partial class BiomeCamera : Camera2D
{
    [Export] public float MinX = 0f;
    [Export] public float MaxX = 5000f;
    [Export] public float DragSensitivity = 1.0f;

    private bool _isDragging = false;
    private Vector2 _lastMousePosition = Vector2.Zero;

    public override void _Ready()
    {
        // Center the camera at the start
        Position = new Vector2(GlobalPosition.X, GlobalPosition.Y);
    }

    public override void _Input(InputEvent @event)
    {
        // Handle mouse drag
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed)
                {
                    _isDragging = true;
                    _lastMousePosition = mouseButton.Position;
                }
                else
                {
                    _isDragging = false;
                }
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion && _isDragging)
        {
            Vector2 delta = mouseMotion.Position - _lastMousePosition;
            _lastMousePosition = mouseMotion.Position;
            
            // Move camera in opposite direction of drag (natural scrolling)
            Position = new Vector2(Position.X - delta.X * DragSensitivity, Position.Y);
            
            // Clamp camera position within bounds
            Position = new Vector2(Mathf.Clamp(Position.X, MinX, MaxX), Position.Y);
        }

        // Handle touch input
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed)
            {
                _isDragging = true;
                _lastMousePosition = touch.Position;
            }
            else
            {
                _isDragging = false;
            }
        }
        else if (@event is InputEventScreenDrag drag)
        {
            Vector2 delta = drag.Position - _lastMousePosition;
            _lastMousePosition = drag.Position;
            
            // Move camera in opposite direction of drag (natural scrolling)
            Position = new Vector2(Position.X - delta.X * DragSensitivity, Position.Y);
            
            // Clamp camera position within bounds
            Position = new Vector2(Mathf.Clamp(Position.X, MinX, MaxX), Position.Y);
        }
    }
}
