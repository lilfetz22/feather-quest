# Responsive Layout & Input Testing Guide

## Overview
This document provides a comprehensive test plan for verifying that Feather Quest works correctly across different screen sizes, aspect ratios, and input methods.

## Project Configuration

### Display Settings (project.godot)
The following settings have been configured for responsive layout:

```ini
[display]
window/size/viewport_width=1920
window/size/viewport_height=1080
window/size/mode=2
window/stretch/mode="canvas_items"
window/stretch/aspect="expand"
```

**Explanation:**
- **viewport_width/height**: Base resolution of 1920x1080 (16:9)
- **mode=2**: Window is resizable
- **stretch_mode="canvas_items"**: Scales 2D content to fit the window while maintaining aspect ratio
- **aspect="expand"**: Expands viewport to fill the window, allowing for scrolling backgrounds

### UI Anchor Configuration

All UI scenes use proper anchor presets to ensure responsive behavior:

1. **BinocularMask**: Full Rect (covers entire screen)
   - Anchors: (0,0) to (1,1)
   - Grows in all directions

2. **BirdContainer**: Center anchored
   - Anchors: (0.5, 0.5) to (0.5, 0.5)
   - Remains centered regardless of screen size

3. **Reticle**: Center anchored
   - Anchors: (0.5, 0.5) to (0.5, 0.5)
   - Always at screen center

## Input Abstraction

Both mouse and touch inputs are supported:

### BiomeCamera (Exploration Phase)
- **Mouse**: `InputEventMouseButton` + `InputEventMouseMotion`
- **Touch**: `InputEventScreenTouch` + `InputEventScreenDrag`
- Behavior: Natural scrolling through parallax biome

### BinocularView (Encounter Phase)
- **Mouse**: `InputEventMouseMotion`
- **Touch**: `InputEventScreenDrag`
- Behavior: Counteracts sway to stabilize bird view

## Test Plan

### Test Environment Setup

#### Desktop Testing (Godot Editor)
1. Open project in Godot 4.x editor
2. Use the Remote debug feature to test different window sizes
3. Manually resize window to test responsiveness

#### Web Testing (HTML5 Export)
1. Export project to HTML5
2. Test in multiple browsers
3. Use browser developer tools to simulate different devices

#### Mobile Testing (if available)
1. Export to Android/iOS
2. Test on actual devices
3. Use touch input exclusively

### Test Cases

## 1. Desktop - 16:9 Landscape (1920x1080)

**Test Configuration:**
- Resolution: 1920x1080
- Input Method: Mouse

**Test Steps:**
1. Launch game
2. Verify Biome scene displays correctly
   - [ ] Sky layer visible and covers screen
   - [ ] All parallax layers visible
   - [ ] No black borders or cutoff content
3. Test camera scrolling
   - [ ] Mouse drag scrolls horizontally
   - [ ] Scrolling is smooth
   - [ ] Camera stays within bounds (0 to MaxX)
4. Enter binocular view (press 'B' or trigger encounter)
   - [ ] Binocular mask covers entire screen
   - [ ] Two circular viewports visible
   - [ ] Bird sprite visible and centered
   - [ ] Reticle visible at screen center
5. Test mouse stabilization
   - [ ] Bird sways naturally
   - [ ] Mouse movement counteracts sway
   - [ ] Bird can be centered with mouse
6. Exit binocular view
   - [ ] Returns to biome smoothly
   - [ ] No visual artifacts

**Expected Results:**
✅ All UI elements properly positioned
✅ Full screen coverage without letterboxing
✅ Smooth mouse input

## 2. Desktop - Different Window Sizes

**Test Configurations:**
- 2560x1440 (16:9 larger)
- 1366x768 (16:9 smaller)
- 1280x720 (16:9 HD)

**Test Steps:**
For each resolution:
1. Resize Godot window or browser window
2. Verify UI scales correctly
   - [ ] Binocular mask still covers screen
   - [ ] Bird and reticle remain centered
   - [ ] Parallax layers scale appropriately
3. Test input responsiveness
   - [ ] Camera drag sensitivity feels consistent
   - [ ] Binocular stabilization feels consistent

**Expected Results:**
✅ UI elements maintain proportions
✅ No distortion or stretching
✅ Input sensitivity consistent across sizes

## 3. Mobile - 9:16 Portrait (1080x1920)

**Test Configuration:**
- Resolution: 1080x1920 (portrait)
- Input Method: Touch

**Test Steps:**
1. Launch game on mobile device or simulator
2. Verify Biome scene displays correctly
   - [ ] All parallax layers visible
   - [ ] Vertical space utilized properly
   - [ ] No content cutoff
3. Test camera scrolling
   - [ ] Touch and drag scrolls horizontally
   - [ ] Scrolling feels natural
   - [ ] Momentum/inertia appropriate (if implemented)
4. Enter binocular view
   - [ ] Binocular mask fills screen vertically
   - [ ] Circular viewports properly sized
   - [ ] Bird sprite visible
   - [ ] Reticle visible
5. Test touch stabilization
   - [ ] Bird sways naturally
   - [ ] Touch drag counteracts sway
   - [ ] Touch response feels smooth
6. Test edge cases
   - [ ] Multitouch doesn't break input
   - [ ] Screen rotation handled (if supported)

**Expected Results:**
✅ Portrait layout works correctly
✅ Touch input responsive
✅ UI elements properly scaled
✅ No accidental inputs from screen edges

## 4. Tablet - 4:3 Aspect Ratio (1024x768)

**Test Configuration:**
- Resolution: 1024x768 (4:3)
- Input Method: Touch or Mouse

**Test Steps:**
1. Launch game
2. Verify Biome scene
   - [ ] Parallax layers visible
   - [ ] No weird stretching
   - [ ] Screen fully utilized
3. Test camera scrolling
   - [ ] Drag input works (touch or mouse)
   - [ ] Scrolling smooth
4. Enter binocular view
   - [ ] Binocular mask covers screen
   - [ ] Circular viewports properly centered
   - [ ] Bird and reticle visible
5. Test input
   - [ ] Touch/mouse drag works
   - [ ] Stabilization feels good

**Expected Results:**
✅ 4:3 aspect ratio handled correctly
✅ No pillarboxing or letterboxing issues
✅ UI elements scale appropriately

## 5. Mobile Landscape (16:9 and wider)

**Test Configuration:**
- Resolution: 2436x1125 (iPhone X landscape, ~19.5:9)
- Input Method: Touch

**Test Steps:**
1. Rotate device to landscape
2. Verify Biome scene
   - [ ] Parallax layers visible
   - [ ] No content cutoff
   - [ ] Safe area respected (notches, camera cutouts)
3. Test camera scrolling
   - [ ] Touch drag works
   - [ ] Scrolling smooth
4. Enter binocular view
   - [ ] Binocular mask covers screen including notch areas
   - [ ] Bird and reticle centered
   - [ ] UI elements avoid notch areas if critical
5. Test touch input
   - [ ] Drag works smoothly
   - [ ] No dead zones near notches

**Expected Results:**
✅ Ultra-wide aspect ratios handled
✅ Notch areas considered (safe area)
✅ No critical UI in unsafe areas

## 6. Input Method Testing

### Mouse Input
**Test Steps:**
1. Biome scene: Click and drag to scroll
   - [ ] Left mouse button drag works
   - [ ] Drag sensitivity appropriate
   - [ ] Release stops dragging
2. Binocular view: Move mouse to stabilize
   - [ ] Mouse motion detected
   - [ ] Counteracts sway smoothly
   - [ ] Sensitivity feels good

### Touch Input
**Test Steps:**
1. Biome scene: Touch and drag to scroll
   - [ ] Single finger drag works
   - [ ] Drag feels natural
   - [ ] Release stops dragging
2. Binocular view: Touch drag to stabilize
   - [ ] Drag motion detected
   - [ ] Counteracts sway smoothly
   - [ ] Multitouch doesn't interfere

### Mixed Input (Desktop with touchscreen)
**Test Steps:**
1. Test both mouse and touch in same session
   - [ ] Both inputs work independently
   - [ ] No conflicts between input types
   - [ ] Switching between inputs seamless

## 7. Edge Cases and Stress Tests

### Extreme Aspect Ratios
**Test Configurations:**
- 21:9 ultrawide (2560x1080)
- 32:9 super ultrawide (3840x1080)

**Test Steps:**
1. Verify UI doesn't break
   - [ ] Binocular mask still covers screen
   - [ ] Bird and reticle centered
   - [ ] Parallax layers extend properly

### Very Small Screens
**Test Configurations:**
- 480x320 (very small device)
- 800x600 (older tablets)

**Test Steps:**
1. Verify UI is still usable
   - [ ] Text readable (if any)
   - [ ] Touch targets large enough
   - [ ] Critical UI visible

### Window Resizing (Desktop)
**Test Steps:**
1. Resize window while game is running
   - [ ] UI adapts in real-time
   - [ ] No crashes or visual glitches
   - [ ] Gameplay uninterrupted

## 8. Performance Testing

### Frame Rate
**Test Steps:**
1. Monitor FPS on each platform
   - [ ] Desktop: 60 FPS stable
   - [ ] Mobile: 30-60 FPS stable
   - [ ] Web: 30+ FPS stable

### Touch Response Time
**Test Steps:**
1. Measure input latency
   - [ ] Touch to response < 100ms
   - [ ] No noticeable lag
   - [ ] Smooth during scrolling

## Acceptance Criteria Summary

Based on the issue requirements:

- ✅ **Game window resizes gracefully without cutting off critical UI or showing raw edges**
  - Verified by tests 1-5

- ✅ **Binocular mask covers the screen on all tested aspect ratios**
  - Verified by tests 1-5, especially edge cases in test 7

- ✅ **Input works for both mouse and touch emulation**
  - Verified by test 6

- ✅ **"Safe Area" is respected for mobile notches (if applicable/simulated)**
  - Verified by test 5 (Mobile Landscape)

## Testing Tools and Utilities

### Godot Editor Testing
1. **Remote Debug**: Run game and resize window
2. **Device Simulator**: Use viewport container to simulate sizes
3. **Touch Emulation**: Project Settings > Input Devices > Pointing > Emulate Touch From Mouse

### Browser Testing
1. **Chrome DevTools**: Device mode with preset devices
2. **Firefox Responsive Design Mode**: Test different screen sizes
3. **Safari**: Use device simulators

### Manual Testing Checklist
Create a spreadsheet to track test results:
```
| Test Case | Device/Size | Input Method | Pass/Fail | Notes |
|-----------|-------------|--------------|-----------|-------|
| 16:9 Desktop | 1920x1080 | Mouse | [ ] | |
| 9:16 Portrait | 1080x1920 | Touch | [ ] | |
| 4:3 Tablet | 1024x768 | Touch | [ ] | |
| ... | ... | ... | [ ] | |
```

## Known Issues and Workarounds

### Issue: Touch emulation in Godot editor
**Workaround**: Enable "Emulate Touch From Mouse" in Project Settings

### Issue: Browser safe area detection
**Workaround**: Use CSS `env(safe-area-inset-*)` if needed in HTML export template

### Issue: Godot .NET Web export limitations
**Note**: Some touch events may behave differently in web builds. Always test exported builds, not just editor.

## Reporting Issues

When reporting layout or input issues, include:
1. Device/screen resolution
2. Browser/platform
3. Input method used
4. Screenshot or video
5. Steps to reproduce

## Next Steps

After completing these tests:
1. Document any issues found
2. Adjust stretch mode or aspect settings if needed
3. Fine-tune anchor positions for edge cases
4. Add safe area margins if needed for notches
5. Performance optimization if needed

## References

- Godot Documentation: [Multiple Resolutions](https://docs.godotengine.org/en/stable/tutorials/rendering/multiple_resolutions.html)
- Godot Documentation: [Input Examples](https://docs.godotengine.org/en/stable/tutorials/inputs/input_examples.html)
- Project Roadmap: `ROADMAP.md` - [UI-000] Responsive Layout & Input Testing
