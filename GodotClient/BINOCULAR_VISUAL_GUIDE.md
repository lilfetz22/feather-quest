# Binocular UI Visual Reference

## How It Works

### Visual Layout
```
┌─────────────────────────────────────────────────────┐
│  ████████████████   BLACK MASK   ████████████████   │
│  ████████████████                 ████████████████   │
│  ███████╭─────────╮          ╭─────────╮███████     │
│  ██████│           │          │           │██████    │
│  █████│             │        │             │█████    │
│  ████│    [BIRD]    │  ┼┼┼  │             │████     │
│  █████│             │  ┼┼┼  │             │█████    │
│  ██████│           │  ┼┼┼    │           │██████    │
│  ███████╰─────────╯  ┼┼┼    ╰─────────╯███████     │
│  ████████████████     ┼┼┼     ████████████████      │
│  ████████████████  RETICLE  ████████████████        │
└─────────────────────────────────────────────────────┘
       ↑                 ↑
   Bird moves      Reticle stays
   with sway        centered
```

### Sway Behavior
```
Time: 0.0s          Time: 0.5s          Time: 1.0s
   ┼                   ┼                   ┼
   🐦                 🐦                   🐦
(centered)      (moved left/up)      (moved right/down)

The bird oscillates smoothly using sine waves
Player moves mouse to counteract and keep bird centered
```

### Mouse Control Mechanic
```
Bird Position = Sway Offset - Mouse Offset

Example:
- Sway pushes bird 50px to the right
- Player moves mouse 50px to the right
- Bird appears centered (50 - 50 = 0)
```

## Component Hierarchy

```
BinocularView (CanvasLayer)
│
├─ BinocularMask (TextureRect)
│  └─ [Full screen black overlay with 2 circular cutouts]
│
├─ BirdContainer (Control) ← MOVES WITH SWAY
│  └─ BirdSprite (TextureRect)
│     └─ [Bird image that player is trying to identify]
│
└─ Reticle (Control) ← STAYS CENTERED
   └─ ReticleSprite (TextureRect)
      └─ [Crosshair helping player aim]
```

## Flow Diagram

```
┌─────────────┐
│   Player    │
│ clicks bird │
│     cue     │
└──────┬──────┘
       │
       ▼
┌────────────────────────┐
│  StartEncounter()      │
│  - Show binocular UI   │
│  - Load bird texture   │
│  - Reset timers        │
└──────┬─────────────────┘
       │
       ▼
┌────────────────────────┐
│   Every Frame:         │◄───────┐
│  1. Calculate sway     │        │
│  2. Get mouse input    │        │
│  3. Update position    │        │
│  4. Track focus        │        │
└──────┬─────────────────┘        │
       │                          │
       │ Focus < 100?             │
       ├─ Yes ──────────────────────┘
       │
       │ Focus >= 100?
       └─ Yes ─────────┐
                       ▼
              ┌────────────────────────┐
              │  CompleteFocus()       │
              │  - Calculate quality   │
              │  - Determine medal     │
              │  - End encounter       │
              │  - Show results        │
              └────────────────────────┘
```

## Key Parameters

| Parameter | Default | Effect |
|-----------|---------|--------|
| Stability | 0.5 | 0.0 = max sway, 1.0 = no sway |
| SwayAmplitude | 0.2 | Size of sway movement |
| MouseSensitivity | 1.0 | How responsive mouse is |
| ViewportSize | 400.0 | Scale factor for sway |

## Difficulty Progression

```
Easy    ████████░░ Stability: 0.8, Sway: 0.1
Medium  ██████░░░░ Stability: 0.5, Sway: 0.2
Hard    ████░░░░░░ Stability: 0.2, Sway: 0.3
Expert  ██░░░░░░░░ Stability: 0.0, Sway: 0.4
```

## Photo Quality Scoring

```
Bird Offset: How far from center the bird is
Stability:   Average centering during encounter

Quality = (1 - normalized_distance) × stability

┌────────────┬──────────┬────────┐
│   Quality  │  Medal   │  Score │
├────────────┼──────────┼────────┤
│   ≥ 0.85   │   Gold   │  ⭐⭐⭐  │
│ 0.60-0.85  │  Silver  │  ⭐⭐   │
│ 0.35-0.60  │  Bronze  │  ⭐    │
│   < 0.35   │   None   │  ✗    │
└────────────┴──────────┴────────┘
```

## Example Usage Code

```csharp
// Start an encounter
binocularView.StartEncounter(birdTexture);

// During encounter (every frame)
Vector2Simple offset = binocularView.GetBirdOffsetFromCenter();
float quality = FocusCalculator.CalculatePhotoQuality(offset, stability);

// End encounter
binocularView.EndEncounter();
```

## Testing Shortcuts

- Press **'B'** to toggle binocular view on/off
- Press **ESC** to cancel encounter (with integration example)
- Adjust exported properties in Godot inspector for live tuning

## Architecture Benefits

✅ **Pure Core Logic**: FocusCalculator has no Godot dependencies
✅ **Migration Ready**: Core math stays identical when moving to Unity
✅ **Testable**: 56 unit tests verify sway calculations
✅ **Configurable**: All parameters exposed for tuning
✅ **Documented**: Complete setup and integration guides included
