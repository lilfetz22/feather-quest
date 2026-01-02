# Biome Scene Visual Representation

## Scene Layout (Side View)

```
┌─────────────────────────────────────────────────────────────────┐
│                         SKY LAYER                                │  Motion: 0.0 (Static)
│                      (Light Blue #87CEE8)                        │
│                                                                   │
│                                                                   │
├═══════════════════════════════════════════════════════════════════┤
│       🌲  DISTANT TREES LAYER (Dark Green #4C7F4C)       🌲      │  Motion: 0.2 (Slow)
│   🌲          🌳        🌲        🌳          🌲        🌳        │
├═══════════════════════════════════════════════════════════════════┤
│   🌿  MIDGROUND LAYER (Medium Green #336633)  🌿     🌿         │  Motion: 0.5 (Normal)
│      🌿        🌿           🌿        🌿                          │
├═══════════════════════════════════════════════════════════════════┤
│ 🌱 FOREGROUND LAYER (Darkest Green #222244) 🌱 🌱 🌱            │  Motion: 1.0 (Fast)
└─────────────────────────────────────────────────────────────────┘

        👆 DRAG HERE to scroll left/right
        (Works with mouse or touch)
```

## Parallax Effect Demonstration

When camera moves **100 pixels to the right**:

```
Layer               Motion Scale    Visible Movement    Effect
────────────────────────────────────────────────────────────────
Sky                 0.0             0 pixels            Stays fixed
Distant Trees       0.2             20 pixels           Barely moves
Midground           0.5             50 pixels           Moves moderately
Foreground          1.0             100 pixels          Moves with camera
```

This creates a sense of **depth** - distant objects appear far away, close objects appear nearby.

## Input Flow Diagram

```
┌─────────────────┐
│  User Input     │
│  - Mouse Drag   │
│  - Touch Swipe  │
└────────┬────────┘
         │
         v
┌─────────────────────────────┐
│  BiomeCamera._Input()       │
│  - Detects button/touch     │
│  - Tracks motion/drag       │
│  - Calculates delta         │
└────────┬────────────────────┘
         │
         v
┌─────────────────────────────┐
│  UpdateCameraPosition()     │
│  - Applies delta * sens.    │
│  - Clamps to bounds         │
└────────┬────────────────────┘
         │
         v
┌─────────────────────────────┐
│  Camera Position Updated    │
│  - ParallaxBackground auto  │
│    updates all layers       │
└─────────────────────────────┘
```

## Camera Bounds Visualization

```
MinX = 0                                                MaxX = 5000
 │                                                          │
 v                                                          v
┌──────────────────────────────────────────────────────────┐
│                                                           │
│         ┌─────────┐                                      │
│         │ Camera  │ ← Can move freely within bounds      │
│         │  View   │                                      │
│         └─────────┘                                      │
│                                                           │
└──────────────────────────────────────────────────────────┘

Camera X position is clamped: Mathf.Clamp(Position.X, 0, 5000)
```

## Scene Node Tree

```
Biome (Node2D)
│
├── ParallaxBackground
│   │
│   ├── SkyLayer (ParallaxLayer)
│   │   └── SkyRect (ColorRect 1920x1080)
│   │       Color: RGB(135, 206, 235) - Sky Blue
│   │       Motion: (0, 0) - STATIC
│   │
│   ├── DistantTreesLayer (ParallaxLayer)
│   │   └── DistantTreesRect (ColorRect 1920x480)
│   │       Color: RGB(76, 127, 76) - Dark Green
│   │       Motion: (0.2, 0) - SLOW
│   │       Position: Y=600 to 1080
│   │
│   ├── MidgroundLayer (ParallaxLayer)
│   │   └── MidgroundRect (ColorRect 1920x330)
│   │       Color: RGB(51, 102, 51) - Medium Green
│   │       Motion: (0.5, 0) - NORMAL
│   │       Position: Y=750 to 1080
│   │
│   └── ForegroundLayer (ParallaxLayer)
│       └── ForegroundRect (ColorRect 1920x180)
│           Color: RGB(34, 34, 68) - Darkest Green
│           Motion: (1.0, 0) - FAST
│           Position: Y=900 to 1080
│
└── BiomeCamera (Camera2D)
    Position: (960, 540) - Center of 1920x1080 viewport
    Properties:
    - MinX: 0.0
    - MaxX: 5000.0
    - DragSensitivity: 1.0
```

## Color Palette

```
┌─────────────┐  Sky Layer
│  #87CEE8    │  Light Blue - Open sky
│  ░░░░░░░░░  │
└─────────────┘

┌─────────────┐  Distant Trees
│  #4C7F4C    │  Dark Green - Far forest
│  ▓▓▓▓▓▓▓▓▓  │
└─────────────┘

┌─────────────┐  Midground
│  #336633    │  Medium Green - Middle vegetation
│  ████████   │
└─────────────┘

┌─────────────┐  Foreground
│  #222244    │  Darkest Green - Close vegetation
│  ■■■■■■■■■  │
└─────────────┘
```

## Code Architecture

```
┌─────────────────────────────────────────────┐
│           BiomeCamera.cs                     │
│  ┌────────────────────────────────────────┐ │
│  │  Exported Properties:                  │ │
│  │  • float MinX                          │ │
│  │  • float MaxX                          │ │
│  │  • float DragSensitivity              │ │
│  └────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────┐ │
│  │  Private State:                        │ │
│  │  • bool _isDragging                    │ │
│  │  • Vector2 _lastMousePosition          │ │
│  └────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────┐ │
│  │  Methods:                              │ │
│  │  • _Ready()                            │ │
│  │  • _Input(InputEvent)                  │ │
│  │  • UpdateCameraPosition(Vector2)       │ │
│  └────────────────────────────────────────┘ │
└─────────────────────────────────────────────┘
         │
         │ Controls
         v
┌─────────────────────────────────────────────┐
│        Camera2D (Godot Engine)               │
│  • Position property                         │
│  • Affects ParallaxBackground automatically  │
└─────────────────────────────────────────────┘
```

## Testing Scenarios

### Scenario 1: Mouse Drag Left
```
User Action: Click and drag mouse LEFT
Expected: Camera moves RIGHT (natural scrolling)
Result: View scrolls to show content on the RIGHT side

Before:          After:
┌──────┐        ┌──────┐
│█     │        │    █ │
│ View │   →    │ View │
└──────┘        └──────┘
```

### Scenario 2: Touch Swipe Right
```
User Action: Touch and swipe RIGHT
Expected: Camera moves LEFT (natural scrolling)
Result: View scrolls to show content on the LEFT side

Before:          After:
┌──────┐        ┌──────┐
│    █ │        │█     │
│ View │   ←    │ View │
└──────┘        └──────┘
```

### Scenario 3: Camera Bounds
```
User Action: Drag camera beyond MaxX
Expected: Camera stops at MaxX (5000)
Result: Cannot scroll further, smooth stop

Position:  4900 → 5000 → 5000 (clamped)
           ✓      ✓      ✗ (blocked)
```

## Performance Considerations

- **No physics calculations** - Direct position updates
- **No complex rendering** - Simple ColorRect nodes
- **Efficient input handling** - Only processes while dragging
- **Memory footprint** - Minimal (4 ColorRect nodes)
- **Web-friendly** - Compatible with WebGL export

## Asset Replacement Guide

To replace placeholders with real art:

1. **Create sprite sheets** for each layer
2. **Replace ColorRect** with Sprite2D or TextureRect
3. **Set textures** to your art assets
4. **Adjust motion_mirroring** to match texture width
5. **Keep motion_scale** values unchanged for consistent parallax

Example for Sky Layer:
```gdscript
# Before (placeholder)
[node name="SkyRect" type="ColorRect" parent="ParallaxBackground/SkyLayer"]
color = Color(0.529412, 0.807843, 0.921569, 1)

# After (real art)
[node name="SkySprite" type="Sprite2D" parent="ParallaxBackground/SkyLayer"]
texture = ExtResource("sky_texture")
```

## Future Enhancements

1. **Vertical Parallax** - Add Y-axis motion for hills/clouds
2. **Camera Smoothing** - Lerp camera position for easing
3. **Zoom Controls** - Scale camera for different views
4. **Weather Effects** - Fog, rain layers with particle systems
5. **Time of Day** - Color tinting based on game time
6. **Bird Spawning** - Integration with game logic (next sprint)

---

This visual guide demonstrates how the parallax scrolling biome scene works,
providing a foundation for the player's bird-watching exploration experience.
