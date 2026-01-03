i haag and Drop & Strikes System Fix

## Changes Made

### 1. Drag and Drop System (PlayerController.cs)

**Problem:** Click-to-grab, click-to-drop was confusing and didn't feel like proper drag and drop.

**Solution:** Changed to press-and-hold drag and drop:
- **Press and hold** left mouse button on an item → Grabs the item
- **Drag** while holding → Item follows your cursor
- **Release** mouse button → Drops the item (on injury zone or returns to original position)

**How it works:**
- Tracks mouse button state (pressed/released)
- Only grabs when mouse is first pressed (not already holding)
- Only releases when mouse button is released (not when pressed again)
- Item follows cursor position in 3D space during drag

### 2. Strikes System (FirstAidGameManager.cs)

**Problem:** Strikes weren't appearing on screen.

**Solution:** Added better error checking and debugging:
- Checks if `strikesParent` is assigned (logs error if not)
- Checks if `strikeXPrefab` is assigned (logs error if not)
- Ensures instantiated strike X is active
- Properly sets up UI elements (RectTransform) if strike is a UI element
- Added debug logs to help troubleshoot

## Setup Checklist

### Drag and Drop
- ✅ Already working - just test it!
- Press and hold mouse button on a tool
- Drag it around
- Release over the injury zone to use it

### Strikes System
Make sure these are set up correctly:

1. **Strikes Parent:**
   - In Hierarchy, find or create `StrikesParent` (should be under Canvas)
   - Select `FirstAidGameManager` in Hierarchy
   - Drag `StrikesParent` into `Strikes Parent` field

2. **Strike X Prefab:**
   - Create a UI Image or Text element with an "X"
   - Make it a prefab (drag to Prefabs folder)
   - Select `FirstAidGameManager`
   - Drag the prefab into `Strike X Prefab` field

3. **Test:**
   - Enter Play Mode
   - Use a wrong tool on the injury
   - Check Console for any errors
   - Strike X should appear in the strikes parent

## Troubleshooting

### Drag and Drop Not Working

**Problem:** Item doesn't grab when clicking
- Check Console for errors
- Verify item has `MedicalItem` component
- Verify item has a Collider
- Check `Grab Distance` is large enough

**Problem:** Item doesn't follow cursor
- Item should follow cursor smoothly
- If it doesn't move, check camera is assigned
- Adjust `distanceFromCamera` in `UpdateHeldItemPosition` if needed

**Problem:** Item doesn't drop on injury zone
- Make sure you're releasing over the injury zone
- Check InjuryZone has a collider
- Verify collider is large enough
- Check Console for drop messages

### Strikes Not Appearing

**Check Console:**
- Look for error messages:
  - "Strikes Parent is not assigned" → Assign it in Inspector
  - "Strike X Prefab is not assigned" → Assign prefab in Inspector

**Verify Setup:**
1. `Strikes Parent` is assigned in FirstAidGameManager
2. `Strike X Prefab` is assigned in FirstAidGameManager
3. `Strikes Parent` is a UI element (child of Canvas)
4. `Strike X Prefab` is a UI element (Image or Text)
5. Prefab is set up correctly (has RectTransform if UI)

**Test:**
1. Enter Play Mode
2. Use a wrong tool (one that doesn't match current step)
3. Check Console - should see "Strike X added!" message
4. Check `StrikesParent` in Hierarchy - should have new child objects

**Common Issues:**
- Prefab is inactive → Make sure prefab is active
- Prefab is not UI element → Use UI Image or Text
- Parent is not UI element → Make sure it's under Canvas
- Prefab doesn't have RectTransform → Add RectTransform component
- Position is wrong → Check RectTransform settings on prefab

## Testing

### Test Drag and Drop:
1. Enter Play Mode
2. Click and hold on a tool in the first aid kit
3. Tool should be grabbed and follow cursor
4. Drag tool over the injury zone
5. Release mouse button
6. Tool should be used (if correct) or return (if wrong)

### Test Strikes:
1. Enter Play Mode
2. Progress to Step 2 (after tapping injury)
3. Use a wrong tool (e.g., use bandage when you need washcloth)
4. Strike X should appear in top-right (or wherever strikes parent is)
5. Use wrong tool 2 more times
6. After 3 strikes, Fail Panel should appear

## Debug Messages

The system now logs helpful messages:

**Drag and Drop:**
- "Grabbed item: [name] (Tag: [tag])" - When item is grabbed
- "[Item] dropped in zone!" - When dropped correctly
- "Wrong tool [name] used! Strike added." - When wrong tool used

**Strikes:**
- "Strike [number] added! Strike X instantiated at strikes parent." - When strike is added
- Error messages if setup is incorrect

Check the Console window to see these messages and troubleshoot issues.

