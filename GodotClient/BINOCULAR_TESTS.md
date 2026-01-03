# Binocular UI Test Plan

## Automated Tests

### Core Logic Tests (Already Passing)
All FocusCalculator tests in `Tests/FocusCalculatorTests.cs` verify the sway mechanics:
- ✅ Sway increases with lower stability
- ✅ Sway is deterministic (same inputs = same outputs)
- ✅ Sway changes smoothly over time
- ✅ X and Y axes are independent
- ✅ Custom amplitude affects sway magnitude
- ✅ Stability values are clamped to [0, 1]

### Integration Tests
The following can be verified programmatically:
- Vector2Simple to Godot Vector2 conversion works correctly
- Mouse offset calculations are correct
- Bird position = Sway - MouseOffset formula works as expected

## Manual Testing Checklist

### Basic Functionality
- [ ] Scene loads without errors
- [ ] Binocular view is initially hidden
- [ ] Pressing 'B' toggles the view on/off
- [ ] Bird sprite is visible when view is active
- [ ] Reticle stays centered on screen

### Sway Mechanics
- [ ] Bird moves independently of the mask
- [ ] Movement is smooth (no stuttering)
- [ ] Movement is continuous (no sudden jumps)
- [ ] X and Y movement are independent (not circular)
- [ ] Sway amplitude matches exported property

### Mouse Input
- [ ] Moving mouse affects bird position
- [ ] Mouse movement counteracts sway effectively
- [ ] Player can stabilize the bird at center with mouse
- [ ] Mouse offset is clamped (can't move infinitely)
- [ ] Mouse sensitivity setting affects responsiveness

### Stability Testing
- [ ] Set Stability = 0.0: Maximum sway observed
- [ ] Set Stability = 0.5: Moderate sway observed
- [ ] Set Stability = 1.0: No sway observed
- [ ] Stability changes take effect immediately

### Visual Quality
- [ ] Binocular mask renders correctly
- [ ] Circular cutouts are properly positioned
- [ ] Bird sprite is clear and visible
- [ ] Reticle is visible against background
- [ ] No z-fighting or rendering artifacts

### Performance
- [ ] Runs smoothly at 60 FPS
- [ ] No memory leaks during extended use
- [ ] CPU usage is reasonable
- [ ] Works on target WebGL platform

## Test Scenarios

### Scenario 1: Easy Encounter (High Stability)
**Setup**: Stability = 0.8, SwayAmplitude = 0.1
**Expected**: Bird moves gently, easy to keep centered
**Result**: Pass/Fail

### Scenario 2: Medium Encounter (Medium Stability)
**Setup**: Stability = 0.5, SwayAmplitude = 0.2
**Expected**: Moderate challenge, requires active mouse control
**Result**: Pass/Fail

### Scenario 3: Hard Encounter (Low Stability)
**Setup**: Stability = 0.2, SwayAmplitude = 0.3
**Expected**: Significant sway, challenging to stabilize
**Result**: Pass/Fail

### Scenario 4: Expert Encounter (No Stability)
**Setup**: Stability = 0.0, SwayAmplitude = 0.4
**Expected**: Maximum sway, very difficult to center
**Result**: Pass/Fail

### Scenario 5: Low Mouse Sensitivity
**Setup**: MouseSensitivity = 0.3
**Expected**: Requires large mouse movements to counteract sway
**Result**: Pass/Fail

### Scenario 6: High Mouse Sensitivity
**Setup**: MouseSensitivity = 2.0
**Expected**: Small mouse movements have large effect
**Result**: Pass/Fail

## Integration Testing

### With Focus Calculator
- [ ] GetBirdOffsetFromCenter() returns accurate values
- [ ] CalculatePhotoQuality() produces expected scores:
  - Centered + High Stability = Gold (0.85+)
  - Slightly Off + Medium Stability = Silver (0.60-0.85)
  - Off-Center + Low Stability = Bronze (0.35-0.60)
  - Very Off + Any Stability = Fail (<0.35)

### With Game State
- [ ] StartEncounter() properly initializes state
- [ ] EndEncounter() properly cleans up
- [ ] Toggle() switches between states correctly
- [ ] Multiple encounters in sequence work correctly
- [ ] State persists correctly during encounter

## Accessibility Testing

### Input Methods
- [ ] Mouse input works correctly
- [ ] Touchpad input works correctly
- [ ] Touch input works on mobile (if supported)
- [ ] Keyboard alternative available (optional)

### Visual Accessibility
- [ ] Reticle is visible for colorblind users
- [ ] Contrast is sufficient for low vision
- [ ] UI elements are readable
- [ ] Motion can be reduced for motion sensitivity (optional)

## Browser Compatibility (WebGL)

- [ ] Chrome/Chromium
- [ ] Firefox
- [ ] Safari
- [ ] Edge
- [ ] Mobile Safari (iOS)
- [ ] Chrome Mobile (Android)

## Known Limitations

1. **No .tscn file**: Scene must be built manually in Godot editor or using BinocularSceneBuilder
2. **Asset paths**: Require Godot project context to resolve `res://` paths
3. **Input handling**: Relies on Godot's input system
4. **Rendering**: Requires Godot rendering pipeline

## Future Test Additions

- [ ] Automated UI tests using Godot's test framework
- [ ] Screenshot comparison tests for visual regression
- [ ] Performance profiling tests
- [ ] Focus meter UI tests
- [ ] Patience meter UI tests
- [ ] Bird flush mechanic tests
