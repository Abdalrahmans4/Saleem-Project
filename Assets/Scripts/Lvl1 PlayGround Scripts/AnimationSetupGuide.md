# Injured Character Animation Setup Guide

This guide explains how to add the "Fallen Idle" animation to your injured character (boy3ForUnity) so it appears to be laying on the ground.

## Overview

The system uses the `InjuredCharacterAnimator` script to play the fallen idle animation automatically when the game starts. The animation will play continuously, showing the character in a fallen/injured position on the ground.

## Step-by-Step Setup

### PART 1: Prepare Your Animation

1. **Locate Your Animation:**
   - In Project window, find your "Fallen Idle" animation
   - It should be in `Assets/Animation/Animation lvl1/` or similar
   - The animation file should be an `.fbx` file or an animation clip

2. **Check Animation Import Settings:**
   - Select the animation file in Project window
   - In Inspector, check:
     - **Animation Type:** Should be `Human` or `Generic` (depending on your rig)
     - **Rig:** Should match your character's rig type
     - **Import Animation:** Should be checked ✅

### PART 2: Create or Configure Animator Controller

#### Option A: Create New Animator Controller (Recommended)

1. **Create the Controller:**
   - In Project window, navigate to where you want to save it (e.g., `Assets/Animiaor/`)
   - Right-click → `Create` → `Animator Controller`
   - Name it: `Boy3InjuredController` or `InjuredCharacterController`

2. **Open the Controller:**
   - Double-click the controller to open Animator window
   - You should see an empty state machine

3. **Add the Fallen Idle State:**
   - Right-click in the Animator window (empty space)
   - Select `Create State` → `Empty`
   - Name it: `Fallen Idle` (or match your animation name exactly)
   - Select the state (it should be orange, meaning it's the default state)

4. **Assign the Animation:**
   - With the state selected, look at Inspector
   - Find `Motion` field
   - Drag your "Fallen Idle" animation clip into this field
   - **OR** click the circle icon and select the animation from the list

5. **Set as Default State:**
   - Right-click on the "Fallen Idle" state
   - Select `Set as Layer Default State`
   - The state should turn orange (default state color)

6. **Configure Animation Settings:**
   - Select the "Fallen Idle" state
   - In Inspector:
     - **Speed:** Set to `1` (normal speed) or adjust as needed
     - **Motion Time:** Leave at 0
     - **Mirror:** Unchecked (unless you need it)
     - **Cycle Offset:** Leave at 0
     - **Foot IK:** Unchecked (usually)
     - **Write Defaults:** Checked ✅ (recommended)

#### Option B: Use Existing Animator Controller

If your character already has an Animator Controller:

1. **Select the Character:**
   - Find `boy3ForUnity` in Hierarchy
   - Check if it has an `Animator` component
   - Note the `Controller` field value

2. **Open the Controller:**
   - Double-click the controller in Project window
   - Animator window opens

3. **Add Fallen Idle State:**
   - Follow steps 3-6 from Option A above
   - Make sure to set it as the default state if you want it to play immediately

### PART 3: Set Up the Character GameObject

1. **Select the Character:**
   - In Hierarchy, select `boy3ForUnity` (or your injured character)

2. **Add Animator Component (if not present):**
   - In Inspector, click `Add Component`
   - Search for `Animator`
   - Click to add it

3. **Configure Animator Component:**
   - **Controller:** Drag your `Boy3InjuredController` (or the controller you created) into this field
   - **Avatar:** Should auto-assign if using Humanoid rig
   - **Apply Root Motion:** Usually unchecked ❌
   - **Update Mode:** `Normal` (default)
   - **Culling Mode:** `Always Animate` (so animation plays even when off-screen)

4. **Add InjuredCharacterAnimator Script:**
   - In Inspector, click `Add Component`
   - Search for `Injured Character Animator`
   - Click to add it

5. **Configure InjuredCharacterAnimator:**
   - **Character Animator:** 
     - Should auto-find the Animator component
     - **OR** drag the Animator component into this field
   - **Fallen Idle State Name:** 
     - Type exactly: `"Fallen Idle"` (must match the state name in Animator Controller)
     - **Important:** This must match the state name exactly (case-sensitive)
   - **Play On Start:** ✅ Check this (animation plays when game starts)
   - **Animation Speed:** `1` (normal speed, adjust if needed)

### PART 4: Integrate with FirstAidGameManager

1. **Select FirstAidGameManager:**
   - In Hierarchy, select `FirstAidGameManager` GameObject

2. **Assign Character Animator:**
   - In Inspector, find `First Aid Game Manager` component
   - Find `Character Animator` field (under Character Setup)
   - Drag the `InjuredCharacterAnimator` component from the character into this field
   - **OR** drag the character GameObject (it will find the component automatically)

3. **Verify Setup:**
   - The game manager will automatically play the animation when the game starts
   - No additional configuration needed!

### PART 5: Test the Animation

1. **Enter Play Mode:**
   - Click Play button in Unity
   - The character should immediately play the "Fallen Idle" animation

2. **Verify Animation:**
   - Character should be in a fallen/laying position
   - Animation should loop continuously
   - Character should remain in this position throughout the game

3. **Check Console:**
   - Open Console window (Window → General → Console)
   - Look for: "Playing animation: Fallen Idle"
   - If you see errors, check the troubleshooting section

## Alternative: Quick Setup Without Animator Controller

If you want a simpler setup without creating an Animator Controller:

1. **Add Animator Component to Character:**
   - Select character in Hierarchy
   - Add `Animator` component
   - Leave `Controller` field empty (or use a minimal controller)

2. **Use Animation Clip Directly:**
   - You can also use `Animation` component instead of `Animator`
   - Add `Animation` component
   - Drag the "Fallen Idle" clip into the component
   - Check `Play Automatically`

**Note:** The `InjuredCharacterAnimator` script is designed for Animator Controller, but you can modify it to work with Animation component if needed.

## Troubleshooting

### Problem: Animation Not Playing

**Symptoms:** Character doesn't animate, stays in default pose

**Solutions:**
1. Check Animator Controller is assigned to Animator component
2. Check state name matches exactly (case-sensitive) in `Fallen Idle State Name`
3. Verify animation clip is assigned to the state in Animator Controller
4. Check Animator component is enabled
5. Verify "Fallen Idle" state is set as default state
6. Check Console for errors
7. Verify animation clip is not empty or corrupted
8. Check if character has the correct Avatar (for Humanoid animations)

### Problem: Wrong Animation Playing

**Symptoms:** Different animation plays, or character animates incorrectly

**Solutions:**
1. Verify animation clip is the correct one (check in Project window)
2. Check state name in Animator Controller matches `Fallen Idle State Name`
3. Verify no other states are set as default
4. Check if there are transitions interfering (remove unwanted transitions)
5. Verify animation is compatible with character's rig

### Problem: Animation Plays But Character Position is Wrong

**Symptoms:** Animation plays but character is floating or in wrong position

**Solutions:**
1. Check character's position in Scene view
2. Verify animation doesn't have root motion that's moving the character
3. Check if character needs to be rotated (some animations assume specific orientation)
4. Adjust character's Transform position/rotation in Scene
5. Check if animation has translation keys that need to be removed

### Problem: Animation Doesn't Loop

**Symptoms:** Animation plays once then stops

**Solutions:**
1. In Animator Controller, select the "Fallen Idle" state
2. In Inspector, find the animation clip settings
3. Check that animation clip is set to `Loop` mode
4. **OR** in Project window, select the animation clip
5. In Inspector, find `Loop Time` and check it ✅
6. Click `Apply` if needed

### Problem: Script Can't Find Animator

**Symptoms:** Console shows "Animator not found!" warning

**Solutions:**
1. Verify Animator component is on the character GameObject
2. Check `Character Animator` field is assigned in InjuredCharacterAnimator
3. Verify character GameObject is the one with the Animator
4. Check if Animator is on a child object (script searches children automatically)

### Problem: State Name Not Found

**Symptoms:** Console shows warning about state name

**Solutions:**
1. Verify state name in Animator Controller exactly matches `Fallen Idle State Name`
2. Check for typos or extra spaces
3. State names are case-sensitive: "Fallen Idle" ≠ "fallen idle"
4. Open Animator window and check the exact state name
5. Copy the state name and paste it into the script field

## Animation State Name Reference

Common names for fallen/idle animations:
- `"Fallen Idle"`
- `"Idle Fallen"`
- `"Fallen"`
- `"Laying Down"`
- `"Injured Idle"`
- `"Ground Idle"`

**Important:** The name must match **exactly** what you named the state in your Animator Controller.

## Advanced Configuration

### Multiple Animation Layers

If you want to layer animations (e.g., breathing on top of fallen pose):

1. In Animator Controller, add a new layer
2. Set layer weight and blending mode
3. Add animations to the new layer
4. Configure transitions between layers

### Animation Events

To trigger events during animation:

1. Select animation clip in Project window
2. In Inspector, find `Events` section
3. Add events at specific frames
4. Create a script to handle the events

### Root Motion

If your animation includes root motion (character moves):

1. In Animator component, check `Apply Root Motion`
2. Character will move according to animation
3. May need to adjust character position in Scene

## Quick Checklist

- [ ] Animation file imported correctly
- [ ] Animator Controller created/configured
- [ ] "Fallen Idle" state added to controller
- [ ] Animation clip assigned to state
- [ ] State set as default
- [ ] Animator component added to character
- [ ] Controller assigned to Animator
- [ ] InjuredCharacterAnimator script added
- [ ] State name matches exactly
- [ ] FirstAidGameManager has character animator assigned
- [ ] Animation plays in Play Mode
- [ ] Animation loops continuously

## Integration with Game Flow

The animation system is integrated with FirstAidGameManager:

- **On Game Start:** Animation plays automatically
- **During Gameplay:** Animation continues playing (character stays fallen)
- **On Game Win:** Animation continues (optional: you can add a "getting up" animation)
- **On Game Lose:** Animation continues

The character will remain in the fallen position throughout the entire first aid treatment process, which is perfect for the game's narrative.

## Next Steps

After setting up the animation:

1. Test the complete game flow
2. Adjust animation speed if needed
3. Fine-tune character position if animation doesn't align perfectly
4. Consider adding transition animations (falling down, getting up) for future enhancements

Good luck with your animation setup!

