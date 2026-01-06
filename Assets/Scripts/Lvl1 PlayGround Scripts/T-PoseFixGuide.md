# Fixing T-Pose Issue - Quick Guide

If your character is stuck in T-pose, follow these steps in order:

## Quick Fix Checklist

### Step 1: Verify Animator Controller is Assigned

1. **Select `boy3forunity` in Hierarchy**
2. **Check Animator Component:**
   - Is `Controller` field assigned? (Should show `Boy3InjuredController`)
   - If empty, drag your controller into this field
3. **Check Avatar:**
   - Should show an Avatar (e.g., `X Bot@Fallen IdleAvatar`)
   - If it says "None" or shows an error, see Step 4

### Step 2: Verify Animation Clip is Assigned to State

1. **Open Animator Window:**
   - Double-click `Boy3InjuredController` in Project window
   - Animator window opens

2. **Select "Fallen Idle" State:**
   - Click on the orange "Fallen Idle" state in Animator window

3. **Check Inspector:**
   - Look for `Motion` field
   - **Is it empty?** → This is the problem!
   - **Fix:** Drag `X Bot@Fallen Idle` animation clip into the `Motion` field
   - OR click the circle icon and select the animation

4. **Verify Animation Settings:**
   - `Speed`: Should be 1
   - `Motion Time`: Should be 0
   - `Write Defaults`: Should be checked ✅

### Step 3: Verify State Name Matches

1. **In Animator Window:**
   - Check the exact name of the state (should be "Fallen Idle")

2. **In InjuredCharacterAnimator Script:**
   - `Fallen Idle State Name` should be exactly: `"Fallen Idle"`
   - Must match exactly (case-sensitive, no extra spaces)

### Step 4: Fix Avatar Configuration

**If Avatar shows error or is None:**

1. **Select the Character Model:**
   - In Project, find `boy3forunity.fbx`
   - Select it

2. **Check Import Settings:**
   - In Inspector, find `Rig` tab
   - **Animation Type:** Should be `Humanoid`
   - **Avatar Definition:** Should be `Create From This Model`
   - Click `Apply`

3. **Configure Avatar:**
   - Click `Rig` tab
   - Click `Configure...` button
   - Unity opens Avatar configuration window
   - Check if bones are mapped correctly (should be mostly green)
   - Click `Done`

4. **Re-assign Avatar:**
   - Select `boy3forunity` in Hierarchy
   - In Animator component, assign the Avatar (should auto-populate)

### Step 5: Verify Animation Clip Settings

1. **Select Animation Clip:**
   - In Project, find `X Bot@Fallen Idle` (or your fallen idle animation)
   - Select it

2. **Check Import Settings:**
   - **Animation Type:** Should match character (usually `Humanoid`)
   - **Rig:** Should match character's rig
   - **Import Animation:** Should be checked ✅
   - **Loop Time:** Should be checked ✅ (for looping)

3. **Check Avatar:**
   - Under `Rig` tab, `Avatar` should be assigned
   - Should match the character's avatar

4. **Click `Apply`** if you made changes

### Step 6: Verify State is Default

1. **In Animator Window:**
   - Right-click on "Fallen Idle" state
   - Select `Set as Layer Default State`
   - State should turn orange

### Step 7: Test in Play Mode

1. **Enter Play Mode:**
   - Click Play button
   - Character should play animation

2. **Check Console:**
   - Open Console (Window → General → Console)
   - Look for messages:
     - ✅ "Successfully playing 'Fallen Idle' animation" = Working!
     - ❌ "Animation state mismatch" = See troubleshooting below

## Common Issues and Solutions

### Issue 1: Animation Clip Not Assigned

**Symptom:** State exists but has no motion assigned

**Fix:**
1. Open Animator Controller
2. Select "Fallen Idle" state
3. Drag animation clip into `Motion` field

### Issue 2: Avatar Mismatch

**Symptom:** Avatar shows error or animation doesn't apply

**Fix:**
1. Ensure character model has Humanoid rig
2. Configure Avatar (see Step 4 above)
3. Ensure animation clip uses same Avatar

### Issue 3: Animation Type Mismatch

**Symptom:** Animation doesn't play, character stays in T-pose

**Fix:**
1. Character model: `Animation Type` = `Humanoid`
2. Animation clip: `Animation Type` = `Humanoid`
3. Both must match!

### Issue 4: State Name Mismatch

**Symptom:** Console shows "Animation state mismatch"

**Fix:**
1. Check exact state name in Animator Controller
2. Copy the exact name
3. Paste into `Fallen Idle State Name` field
4. Must match exactly (case-sensitive)

### Issue 5: Animation Not Looping

**Symptom:** Animation plays once then stops

**Fix:**
1. Select animation clip in Project
2. In Inspector, check `Loop Time` ✅
3. Click `Apply`

### Issue 6: Animator Disabled

**Symptom:** Nothing happens

**Fix:**
1. Select character in Hierarchy
2. Check Animator component
3. Ensure component is enabled (checkbox at top)

## Debug Steps

If still not working, check these:

1. **Console Messages:**
   - Open Console window
   - Look for errors or warnings
   - Read the messages carefully

2. **Animator Parameters:**
   - Open Animator Controller
   - Check if there are any parameters that need to be set
   - Check if there are transitions blocking the state

3. **Animation Preview:**
   - Select animation clip in Project
   - In Inspector, click play button in preview window
   - Does animation look correct? If not, animation file might be corrupted

4. **Test with Simple Setup:**
   - Create a new Animator Controller
   - Add only "Fallen Idle" state
   - Assign animation clip
   - Set as default
   - Assign to character
   - Test if this works

## Quick Test Method

1. **Select character in Hierarchy**
2. **In Animator component, check:**
   - ✅ Controller assigned
   - ✅ Avatar assigned
   - ✅ Component enabled

3. **Enter Play Mode**
4. **In Animator window (while playing):**
   - You should see the state highlighted/active
   - If "Fallen Idle" is highlighted, animation should be playing
   - If not highlighted, state isn't active

5. **Check Console:**
   - Should see "Successfully playing" message
   - If you see warnings, follow the instructions in the warning

## Still Not Working?

If none of the above fixes work:

1. **Verify Animation File:**
   - Try playing the animation in a 3D software
   - Ensure animation is not corrupted

2. **Try Different Animation:**
   - Test with a known working animation
   - If that works, issue is with the Fallen Idle animation

3. **Check Character Rig:**
   - Ensure character has proper bone structure
   - Verify all bones are properly connected

4. **Re-import Assets:**
   - Right-click character model → `Reimport`
   - Right-click animation → `Reimport`

## Success Indicators

You'll know it's working when:
- ✅ Character is in fallen/laying position (not T-pose)
- ✅ Animation loops continuously
- ✅ Console shows "Successfully playing" message
- ✅ In Animator window, "Fallen Idle" state is highlighted during play

Good luck!

