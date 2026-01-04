# Scenes Documentation

## Overview
This directory contains all Godot scene files (.tscn) for Feather Quest. Each scene is documented below with its purpose and usage.

## Scene Files

### Biome.tscn
The main exploration scene with scrolling parallax background.

**Purpose**: Forest biome environment where players search for birds

**Features**:
- Parallax background with 4 layers (sky, distant trees, midground, foreground)
- Camera controls with mouse drag and touch swipe
- Horizontal scrolling with bounds
- Natural scrolling behavior

**Controller**: `Scripts/BiomeCamera.cs`

**Documentation**: See detailed parallax implementation below

---

### BinocularUI.tscn
The binocular encounter UI scene.

**Purpose**: UI overlay for bird identification encounters (the "Battle" phase)

**Features**:
- Binocular mask overlay (full screen coverage)
- Bird sprite with sway mechanics
- Center reticle for targeting
- Responsive anchors for all aspect ratios
- Support for mouse and touch input

**Controller**: `Scripts/BinocularView.cs`

**Documentation**: See `BINOCULAR_SETUP.md` and `BINOCULAR_IMPLEMENTATION.md`

---

### ResponsiveTest.tscn
Test scene for responsive layout and input testing.

**Purpose**: Manual testing scene for verifying responsive behavior across different screen sizes

**Features**:
- Includes Biome scene
- Includes BinocularUI scene
- Debug overlay showing resolution and aspect ratio
- Keyboard controls for testing (B, I, F)
- Real-time viewport information

**Controller**: `Scripts/ResponsiveTestController.cs`

**Documentation**: See `RESPONSIVE_TESTING.md` and `RESPONSIVE_IMPLEMENTATION.md`

**Usage**:
1. Open scene in Godot
2. Run with F5
3. Press 'B' to toggle binocular view
4. Press 'I' to refresh viewport info
5. Press 'F' to toggle fullscreen
6. Resize window to test responsiveness

---

## Biome Scene - Detailed Documentation

## Overview
This scene implements a scrolling parallax background for the forest biome environment where players can search for birds.

## Files
- **Scenes/Biome.tscn** - Main biome scene with parallax background
- **Scripts/BiomeCamera.cs** - Camera controller for input handling

## Scene Structure

```
Biome (Node2D)
├── ParallaxBackground
│   ├── SkyLayer (ParallaxLayer - Motion Scale: 0, 0)
│   │   └── SkyRect (ColorRect - Sky Blue)
│   ├── DistantTreesLayer (ParallaxLayer - Motion Scale: 0.2, 0)
│   │   └── DistantTreesRect (ColorRect - Dark Green)
│   ├── MidgroundLayer (ParallaxLayer - Motion Scale: 0.5, 0)
│   │   └── MidgroundRect (ColorRect - Medium Green)
│   └── ForegroundLayer (ParallaxLayer - Motion Scale: 1.0, 0)
│       └── ForegroundRect (ColorRect - Darkest Green)
└── BiomeCamera (Camera2D)
```

## Features

### Parallax Layers
1. **Sky Layer** - Static background (Motion Scale 0,0)
   - Light blue color representing the sky
   - No movement to create depth

2. **Distant Trees Layer** - Slow movement (Motion Scale 0.2, 0)
   - Dark green representing distant forest
   - Moves at 20% of camera speed

3. **Midground Layer** - Normal movement (Motion Scale 0.5, 0)
   - Medium green for middle-distance vegetation
   - Moves at 50% of camera speed

4. **Foreground Layer** - Fast movement (Motion Scale 1.0, 0)
   - Darkest green for close vegetation
   - Moves at 100% of camera speed (1:1 with camera)

### Camera Controls

#### BiomeCamera.cs Properties
- **MinX** (default: 0) - Minimum X position for camera bounds
- **MaxX** (default: 5000) - Maximum X position for camera bounds
- **DragSensitivity** (default: 1.0) - Multiplier for drag input

#### Input Methods
1. **Mouse Drag**
   - Click and hold left mouse button
   - Drag horizontally to scroll
   - Natural scrolling (drag right to view left content)

2. **Touch Swipe**
   - Touch and drag on touch screen
   - Swipe horizontally to scroll
   - Same natural scrolling behavior

### Camera Clamping
The camera automatically clamps its position to stay within MinX and MaxX bounds, preventing the view from scrolling beyond the biome boundaries.

## Usage

### Opening the Scene
1. Open Godot Editor
2. Navigate to `res://Scenes/Biome.tscn`
3. Press F5 to run the scene

### Testing Parallax Effect
1. Click and drag left/right with mouse
2. Observe different layer speeds
3. Notice the depth effect created by motion scales

### Customizing Bounds
Adjust the BiomeCamera properties in the inspector:
- Increase MaxX for longer biomes
- Adjust DragSensitivity for faster/slower scrolling

## Asset Replacement
Current implementation uses ColorRect placeholders:
- Sky: Light blue (#87CEE8)
- Distant Trees: Dark green (#4C7F4C)  
- Midground: Medium green (#336633)
- Foreground: Darkest green (#222244)

To replace with actual art assets:
1. Replace ColorRect nodes with Sprite2D or TextureRect nodes
2. Assign texture assets to each node
3. Adjust motion_mirroring values to match texture widths
4. Keep the same motion_scale values for consistent parallax effect

## Architecture Notes
This is pure "View" layer implementation:
- No game logic (bird spawning, scoring, etc.)
- No Core project dependencies
- Follows MVP pattern - ready for future Unity migration
- Input handling is isolated in BiomeCamera.cs
- Scene structure supports easy asset swapping

## Future Enhancements
- Add actual forest art assets
- Implement vertical camera movement if needed
- Add smooth camera easing/lerping
- Add zoom controls
- Connect to game state management
