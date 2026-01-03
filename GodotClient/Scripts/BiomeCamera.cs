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

    private bool _isMouseDragging = false;
    private bool _isTouchDragging = false;
    private Vector2 _lastMousePosition = Vector2.Zero;
    private Vector2 _lastTouchPosition = Vector2.Zero;

    private void UpdateCameraPosition(Vector2 delta)
    {
        // Move camera in opposite direction of drag (natural scrolling)
        Position = new Vector2(Position.X - delta.X * DragSensitivity, Position.Y);
        
        // Clamp camera position within bounds
        Position = new Vector2(Mathf.Clamp(Position.X, MinX, MaxX), Position.Y);
    }

    public override void _Ready()
    {
        // Keep the camera position as configured in the scene; no explicit re-initialization needed.
    }
    public override void _Input(InputEvent @event)
    {
        // Handle mouse drag
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed && !_isTouchDragging)
                {
                    _isMouseDragging = true;
                    _lastMousePosition = mouseButton.Position;
                }
                else
                {
                    _isMouseDragging = false;
                }
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion && _isMouseDragging)
        {
            Vector2 delta = mouseMotion.Position - _lastMousePosition;
            _lastMousePosition = mouseMotion.Position;
            UpdateCameraPosition(delta);
        }

        // Handle touch input
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed && !_isMouseDragging)
            {
                _isTouchDragging = true;
                _lastTouchPosition = touch.Position;
            }
            else
            {
                _isTouchDragging = false;
            }
        }
        else if (@event is InputEventScreenDrag drag && _isTouchDragging)
        {
            Vector2 delta = drag.Position - _lastTouchPosition;
            _lastTouchPosition = drag.Position;
            UpdateCameraPosition(delta);
        }
    }
}
