# Quick Start Guide - Responsive Layout Testing

## Prerequisites
- Godot 4.5 or later
- .NET 8.0 SDK

## Quick Test (5 minutes)

### Step 1: Open Project
```bash
# Open in Godot editor
godot --path /path/to/feather-quest/GodotClient
```

### Step 2: Open Test Scene
In Godot editor:
1. Navigate to `res://Scenes/ResponsiveTest.tscn`
2. Double-click to open

### Step 3: Run Scene
1. Press **F5** or click the Play button
2. You should see:
   - Parallax biome background
   - Debug info overlay (top-left)

### Step 4: Test Responsiveness
1. **Resize Window**: Grab window edge and resize
   - Watch the debug info update
   - Notice UI elements scale properly
   
2. **Toggle Binocular View**: Press **'B'**
   - Binocular mask should cover screen
   - Bird sprite should sway
   - Move mouse to stabilize
   
3. **Refresh Info**: Press **'I'**
   - See current resolution
   - See detected aspect ratio
   
4. **Toggle Fullscreen**: Press **'F'**
   - Window switches to fullscreen
   - UI should still scale correctly

### Step 5: Test Different Sizes
Try these common resolutions:
- **1920x1080** (16:9 Desktop) - Resize window to this
- **1080x1920** (9:16 Portrait) - Not testable in editor, use export
- **1024x768** (4:3 Tablet) - Resize window to this
- **2560x1080** (21:9 Ultrawide) - Resize window to this

### Step 6: Test Input Methods

**Mouse Input:**
1. In biome scene: Click and drag to scroll
2. In binocular view: Move mouse to counteract sway

**Touch Input (if touchscreen available):**
1. In biome scene: Touch and drag to scroll
2. In binocular view: Touch drag to counteract sway

## Expected Results

✅ **Window Resizing**
- UI scales smoothly
- No black bars or cutoff content
- Debug info updates correctly

✅ **Binocular View**
- Mask covers entire screen
- Bird stays centered but sways
- Mouse/touch input counteracts sway

✅ **Camera Scrolling**
- Parallax layers move at different speeds
- Camera stays within bounds
- Natural scrolling feel

## Common Issues

### Issue: "BinocularView not found"
**Solution**: Scene may need BinocularUI added. Check ResponsiveTest.tscn structure.

### Issue: Touch not working in editor
**Solution**: 
1. Go to Project Settings
2. Input Devices > Pointing
3. Enable "Emulate Touch From Mouse"

### Issue: Textures not loading
**Solution**: Ensure these files exist:
- `Assets/Textures/binocular_mask.svg`
- `Assets/Textures/placeholder_bird.svg`
- `Assets/Textures/reticle.svg`

## Keyboard Controls

| Key | Action |
|-----|--------|
| **B** | Toggle Binocular View |
| **I** | Refresh Viewport Info |
| **F** | Toggle Fullscreen |
| **ESC** | Exit Scene |

## Mobile/Web Testing

### HTML5 Export
1. Export to HTML5 in Godot
2. Serve with local web server
3. Open in browser
4. Use browser dev tools to simulate devices
5. Test on actual mobile device if available

### Android Export (Advanced)
1. Set up Android export in Godot
2. Export APK
3. Install on Android device
4. Test touch input directly

## Next Steps

For comprehensive testing, see:
- `GodotClient/RESPONSIVE_TESTING.md` - Full test plan
- `GodotClient/RESPONSIVE_IMPLEMENTATION.md` - Implementation details
- `UI-000-SUMMARY.md` - Complete summary

## Need Help?

Check the documentation:
- `GodotClient/Scenes/README.md` - Scene documentation
- `GodotClient/BINOCULAR_SETUP.md` - Binocular UI setup
- `ROADMAP.md` - Project roadmap

## Report Issues

If you find bugs:
1. Note the screen resolution
2. Note the aspect ratio
3. Note the input method (mouse/touch)
4. Take a screenshot
5. Document steps to reproduce

---

**Happy Testing! 🎮**
