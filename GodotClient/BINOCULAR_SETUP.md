# Binocular UI Setup Guide

## Overview
This guide explains how to set up the Binocular UI scene in Godot for the bird identification mechanic.

## Scene Structure

You can create the scene in two ways:

### Option 1: Manual Setup in Godot Editor
Create a scene with the following node hierarchy:

```
Main (Node)
└── BinocularView (CanvasLayer)
    ├── BinocularMask (TextureRect)
    ├── BirdContainer (Control)
    │   └── BirdSprite (TextureRect)
    └── Reticle (Control)
        └── ReticleSprite (TextureRect)
```

### Option 2: Programmatic Setup
Use the `BinocularSceneBuilder.cs` script to create the scene programmatically:
1. Create a new scene with a Node named "Main"
2. Attach the `Scripts/BinocularSceneBuilder.cs` script to the Main node
3. Run the scene - it will build the binocular UI automatically

See `Scripts/BinocularSceneBuilder.cs` for the implementation details.

## Node Configuration

### 1. Main (Node)
- Type: `Node`
- Attach Script: `Scripts/BinocularTestController.cs`

### 2. BinocularView (CanvasLayer)
- Type: `CanvasLayer`
- Attach Script: `Scripts/BinocularView.cs`
- Layer: 100 (to ensure it renders on top)
- **Exported Properties**:
  - `Stability`: 0.5 (0.0 = max sway, 1.0 = no sway)
  - `SwayAmplitude`: 0.2 (maximum sway distance)
  - `MouseSensitivity`: 1.0 (how responsive mouse control is)
  - `ViewportSize`: 400.0 (size of viewing area for calculations)

### 3. BinocularMask (TextureRect)
- Type: `TextureRect`
- Parent: `BinocularView`
- Texture: `Assets/Textures/binocular_mask.svg`
- Layout:
  - Anchors: Full Rect (0,0,1,1)
  - Stretch Mode: Scale
- Purpose: Creates the black overlay with circular cutouts

### 4. BirdContainer (Control)
- Type: `Control`
- Parent: `BinocularView`
- Layout:
  - Anchors: Center (0.5, 0.5, 0.5, 0.5)
  - Position: (0, 0) - Will be moved dynamically by script
  - Size: (400, 400)
- Purpose: Container for the bird sprite that moves with sway

### 5. BirdSprite (TextureRect)
- Type: `TextureRect`
- Parent: `BirdContainer`
- Texture: `Assets/Textures/placeholder_bird.svg`
- Layout:
  - Anchors: Center
  - Position: (0, 0) relative to parent
  - Size: (200, 200)
  - Expand Mode: Keep Size / Ignore Size
- Purpose: Displays the bird being observed

### 6. Reticle (Control)
- Type: `Control`
- Parent: `BinocularView`
- Layout:
  - Anchors: Center (0.5, 0.5, 0.5, 0.5)
  - Position: (0, 0)
  - Size: (100, 100)
- Purpose: Container for the center reticle (stays fixed)

### 7. ReticleSprite (TextureRect)
- Type: `TextureRect`
- Parent: `Reticle`
- Texture: `Assets/Textures/reticle.svg`
- Layout:
  - Anchors: Center
  - Position: (0, 0)
  - Size: (100, 100)
- Purpose: Visual indicator of the center point

## Usage

### Testing the Binocular UI
1. Run the scene
2. Press **'B'** to toggle the binocular view on/off
3. When active, move your mouse to counteract the sway and keep the bird centered

### Integration with Game Logic
```csharp
// To start an encounter with a specific bird texture
BinocularView binocularView = GetNode<BinocularView>("BinocularView");
Texture2D birdTexture = GD.Load<Texture2D>("path/to/bird/texture.png");
binocularView.StartEncounter(birdTexture);

// To end the encounter
binocularView.EndEncounter();

// To get the bird's offset from center (for photo quality calculation)
Vector2Simple offset = binocularView.GetBirdOffsetFromCenter();
float quality = FocusCalculator.CalculatePhotoQuality(offset, averageStability);
```

## Mechanics Explained

### Sway System
- The bird sprite moves based on a sine wave pattern calculated by `FocusCalculator.CalculateSway()`
- The sway is independent on X and Y axes to create realistic hand shake
- Sway amplitude scales with the inverse of stability (lower stability = more sway)

### Mouse Control
- Mouse movement counteracts the sway
- Moving the mouse right moves the bird right (to counteract leftward sway)
- The mouse offset is clamped to prevent excessive movement
- Formula: `BirdPosition = Sway - MouseOffset`

### Focus Calculation
- The `GetBirdOffsetFromCenter()` method returns the bird's position relative to center
- This can be fed into `FocusCalculator.CalculatePhotoQuality()` to determine photo quality
- Perfect centering with high stability = best quality score

## Customization

### Adjusting Difficulty
- **Increase Sway**: Lower the `Stability` value or increase `SwayAmplitude`
- **Make Mouse Control Harder**: Lower `MouseSensitivity`
- **Change Sway Speed**: Modify the frequency values in `FocusCalculator.cs` (currently 1.5 and 2.0)

### Visual Customization
- Replace `binocular_mask.svg` with your own mask texture
- Replace `placeholder_bird.svg` with actual bird photography/sprites
- Modify `reticle.svg` for different crosshair styles
- Add effects like vignette, lens distortion, or chromatic aberration

## Architecture Notes

### MVP Pattern Compliance
- **Model**: `FocusCalculator` in `/Core/Logic` - Pure C# math, no Godot dependencies
- **View**: Godot UI nodes (BinocularMask, BirdSprite, Reticle) - Pure presentation
- **Presenter**: `BinocularView.cs` - Bridges Core logic with Godot UI

### Migration Ready
When migrating to Unity:
- Keep `FocusCalculator.cs` unchanged (already engine-agnostic)
- Replace `BinocularView.cs` with Unity equivalent using Unity UI
- Core sway logic remains 100% portable

## Troubleshooting

### Bird not visible
- Check that BirdContainer and BirdSprite are children of BinocularView
- Verify BirdSprite has a texture assigned
- Check layer ordering (BinocularView should be on top)

### Sway not working
- Ensure the scene is active (`_isActive = true`)
- Verify FocusCalculator is being called in `_Process()`
- Check that `Stability` is not set to 1.0 (which disables sway)

### Mouse control not responding
- Make sure the BinocularView is visible and active
- Check that `_Input()` is being called
- Verify MouseSensitivity is not set to 0

## Next Steps

1. **Add Focus Meter**: Create a UI bar that fills as the bird stays centered
2. **Add Patience Meter**: Timer that counts down if the bird is not in focus
3. **Visual Feedback**: Add particle effects or color changes when well-centered
4. **Sound Effects**: Add audio feedback for successful centering
5. **Integration**: Connect to the game state machine for full encounter flow
