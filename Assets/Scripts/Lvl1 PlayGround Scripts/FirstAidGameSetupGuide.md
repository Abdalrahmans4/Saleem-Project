# First Aid Game Setup Guide - Detailed

This comprehensive guide explains how to set up the complete first aid game system for treating the injured character (boy3ForUnity) with step-by-step instructions.

## Table of Contents
1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Step-by-Step Setup](#step-by-step-setup)
4. [Detailed Configuration](#detailed-configuration)
5. [UI Creation Guide](#ui-creation-guide)
6. [Texture/Material Setup](#texturematerial-setup)
7. [Testing Procedures](#testing-procedures)
8. [Troubleshooting](#troubleshooting)
9. [Common Mistakes](#common-mistakes)

---

## Overview

The game consists of 5 treatment steps that the player must complete in order:
1. **Step 1 (Inspect)**: Player taps on the injured knee - no tool needed
2. **Step 2 (After Washing)**: Player uses a washing tool (e.g., "WashCloth" or "Water")
3. **Step 3 (After Drying)**: Player uses a drying tool (e.g., "Towel" or "DryCloth")
4. **Step 4 (Disinfecting)**: Player uses a disinfectant tool (e.g., "Antiseptic" or "Disinfectant")
5. **Step 5 (Bandage)**: Player uses a bandage tool (e.g., "Bandage")

**Game Mechanics:**
- Correct tool usage → Progress to next step, texture changes
- Wrong tool usage → Wrong tool panel appears, tool disappears, strike added
- 3 strikes → Game over (Fail Panel)
- Complete all 5 steps → Success Panel appears

---

## Prerequisites

Before starting, ensure you have:
- ✅ Unity scene with `boy3ForUnity` character
- ✅ First aid kit with medical tools (at least 4 tools: wash, dry, disinfect, bandage)
- ✅ 5 textures for the injured character (or materials)
- ✅ UI Canvas in your scene
- ✅ PlayerController script attached to player/camera
- ✅ MedicalKit script set up (optional, for opening/closing kit)

---

## Step-by-Step Setup

### PART 1: FirstAidGameManager Setup

#### Step 1.1: Create the Game Manager GameObject

1. **In Unity Hierarchy:**
   - Right-click in the Hierarchy window
   - Select `Create Empty`
   - Name it: `FirstAidGameManager`
   - Position: Doesn't matter (can be at 0,0,0 or anywhere)

2. **Add the Component:**
   - Select the `FirstAidGameManager` GameObject
   - In the Inspector, click `Add Component`
   - Search for `First Aid Game Manager`
   - Click to add it

#### Step 1.2: Find and Assign the Injured Character

1. **Locate the Character:**
   - In Hierarchy, find `boy3ForUnity` (or your injured character)
   - If you can't find it, use the search bar at the top of Hierarchy

2. **Assign to Game Manager:**
   - Select `FirstAidGameManager` in Hierarchy
   - In Inspector, find `First Aid Game Manager` component
   - Find the `Injured Character` field
   - Drag `boy3ForUnity` from Hierarchy into this field

#### Step 1.3: Find and Assign the Character Renderer

**Important:** The renderer is usually on a child object of the character, not the main GameObject.

1. **Locate the Renderer:**
   - Expand `boy3ForUnity` in Hierarchy (click the arrow)
   - Look for child objects like:
     - `Mesh` or `Model` or `Body` or similar
     - Or the character might have multiple parts (head, body, legs, etc.)
   - Select the child object that has the `Mesh Renderer` or `Skinned Mesh Renderer` component
   - This is the object that displays the character's appearance

2. **Verify it's the Right Renderer:**
   - In Inspector, you should see `Mesh Renderer` or `Skinned Mesh Renderer` component
   - This component should have a Material assigned

3. **Assign to Game Manager:**
   - Select `FirstAidGameManager` in Hierarchy
   - In Inspector, find `Character Renderer` field
   - Drag the child object with the Renderer component into this field
   - **Alternative:** You can drag the component itself (the Renderer component) into the field

**Troubleshooting:**
- If you can't find the renderer, select the character and look in Inspector for any Renderer component
- The renderer might be on the main GameObject itself
- You can also use the search in Inspector: type "Renderer" to find it quickly

#### Step 1.4: Configure Treatment Steps

1. **Expand the Treatment Steps Array:**
   - In Inspector, find `Treatment Steps` (under First Aid Game Manager)
   - Click the arrow to expand it
   - You'll see `Size: 0`
   - Change `Size` to `5` and press Enter
   - You should now see 5 elements: `Element 0`, `Element 1`, `Element 2`, `Element 3`, `Element 4`

2. **Configure Element 0 (Step 1 - Inspect):**
   - Expand `Element 0`
   - **Step Name:** Type: `"Inspect"` or `"Step 1: Inspect Injury"`
   - **Character Texture:** 
     - Click the circle icon next to the field
     - Search for: `Boy3ScrappedKnee`
     - Select it
     - **OR** if you have it as a Material instead:
       - Leave Texture empty
       - In `Character Material`, assign `Boy3ScrappedKnee` material
   - **Step Panel:** Leave empty for now (we'll create UI later)
   - **Required Tool Tag:** Leave **completely empty** (no text)
   - **Requires Tap Only:** ✅ **Check this box** (this is important!)

3. **Configure Element 1 (Step 2 - Washing):**
   - Expand `Element 1`
   - **Step Name:** Type: `"Wash"` or `"Step 2: Wash the Wound"`
   - **Character Texture:** Assign `Boy3ScrappedKneeCleanWet`
   - **Step Panel:** Leave empty for now
   - **Required Tool Tag:** Type exactly: `"WashCloth"` (or `"Water"` if that's your tool tag)
     - **Important:** This must match exactly with your tool's tag (case-sensitive)
   - **Requires Tap Only:** ❌ Leave unchecked

4. **Configure Element 2 (Step 3 - Drying):**
   - Expand `Element 2`
   - **Step Name:** Type: `"Dry"` or `"Step 3: Dry the Wound"`
   - **Character Texture:** Assign `Boy3ScrappedKneeCleanDry`
   - **Step Panel:** Leave empty for now
   - **Required Tool Tag:** Type exactly: `"Towel"` (or `"DryCloth"` if that's your tool tag)
   - **Requires Tap Only:** ❌ Leave unchecked

5. **Configure Element 3 (Step 4 - Disinfecting):**
   - Expand `Element 3`
   - **Step Name:** Type: `"Disinfect"` or `"Step 4: Disinfect the Wound"`
   - **Character Texture:** Assign `Boy3ScrappedKneeDisinfected`
   - **Step Panel:** Leave empty for now
   - **Required Tool Tag:** Type exactly: `"Antiseptic"` (or `"Disinfectant"` if that's your tool tag)
   - **Requires Tap Only:** ❌ Leave unchecked

6. **Configure Element 4 (Step 5 - Bandaging):**
   - Expand `Element 4`
   - **Step Name:** Type: `"Bandage"` or `"Step 5: Apply Bandage"`
   - **Character Texture:** Assign `Boy3ScrappedKneeBandaged`
   - **Step Panel:** Leave empty for now
   - **Required Tool Tag:** Type exactly: `"Bandage"`
   - **Requires Tap Only:** ❌ Leave unchecked

**Important Notes:**
- Tool tags are **case-sensitive**: "Bandage" ≠ "bandage" ≠ "BANDAGE"
- Tool tags must match **exactly** between the step and the tool's MedicalItem component
- If you're not sure what tag to use, check your tools first (see Part 3)

#### Step 1.5: Configure UI Panels (Temporary - We'll Create Them Later)

For now, leave these empty. We'll come back after creating the UI:
- **Wrong Tool Panel:** Leave empty
- **Success Panel:** Leave empty
- **Fail Panel:** Leave empty
- **Strikes Parent:** Leave empty
- **Strike X Prefab:** Leave empty

#### Step 1.6: Configure Game Settings

- **Max Strikes:** Set to `3` (default is fine)
- **Wrong Tool Panel Duration:** Set to `2` (default is fine, this is in seconds)

---

### PART 2: InjuryZone Setup

#### Step 2.1: Create or Select the Injury Zone GameObject

**Option A: Use the Character's Knee Area**
1. Select `boy3ForUnity` in Hierarchy
2. Expand it to see child objects
3. Look for a knee/leg object, or create a new child:
   - Right-click on `boy3ForUnity`
   - Select `Create Empty`
   - Name it: `InjuryZone` or `KneeInjuryZone`
   - Position it at the knee location (you may need to adjust in Scene view)

**Option B: Create Separate GameObject**
1. Right-click in Hierarchy
2. Select `Create Empty`
3. Name it: `InjuryZone`
4. Position it near the character's knee in the Scene view

#### Step 2.2: Add Collider to Injury Zone

1. **Select the InjuryZone GameObject**
2. **Add a Collider:**
   - In Inspector, click `Add Component`
   - Search for `Box Collider` (or `Sphere Collider` if you prefer)
   - Click to add it

3. **Configure the Collider:**
   - **Is Trigger:** ✅ **Check this box** (very important!)
   - **Size/Radius:** Adjust to cover the knee area
     - For Box Collider: Set Size to something like (0.3, 0.3, 0.3) or larger
     - For Sphere Collider: Set Radius to 0.15 or larger
   - **Center:** Adjust if needed to center on the knee

4. **Position the Collider:**
   - Switch to Scene view
   - Select the InjuryZone GameObject
   - Use the Move tool (W key) to position it at the knee
   - Use the Scale tool (R key) or adjust collider size to cover the knee area
   - The collider should be visible as a green wireframe in Scene view

#### Step 2.3: Add InjuryZone Component

1. **Select the InjuryZone GameObject**
2. **Add Component:**
   - Click `Add Component`
   - Search for `Injury Zone`
   - Click to add it

#### Step 2.4: Configure InjuryZone Component

1. **Game Manager:**
   - Drag `FirstAidGameManager` from Hierarchy into this field

2. **Tap Collider:**
   - Drag the same Collider component (the one you just added) into this field
   - **OR** if you want a separate collider for tap detection:
     - Create another collider (non-trigger) for tap detection
     - Assign that one here

3. **Allow Tap Interaction:**
   - ✅ **Check this box** (required for Step 1 to work)

4. **Accepted Item Tags:**
   - Leave this **empty** (the game manager handles validation)

5. **Single Item Only:**
   - ✅ **Check this box**

6. **Snap To Center:**
   - ✅ **Check this box**

**Visual Setup Tip:**
- In Scene view, you should see the collider as a green wireframe
- Make sure it's positioned over the knee area
- The collider should be large enough to easily click/drag tools onto

---

### PART 3: Medical Tools Setup

#### Step 3.1: Identify Your Tools

First, identify which tools you have in your first aid kit:
- Washing tool (cloth, water bottle, etc.)
- Drying tool (towel, dry cloth, etc.)
- Disinfecting tool (antiseptic bottle, disinfectant, etc.)
- Bandage tool (bandage, gauze, etc.)

#### Step 3.2: Configure Each Tool

For **EACH** tool in your first aid kit:

1. **Select the Tool GameObject:**
   - Find it in Hierarchy (might be inside Medical_Kit or similar)

2. **Check for MedicalItem Component:**
   - In Inspector, look for `Medical Item` component
   - If it doesn't exist:
     - Click `Add Component`
     - Search for `Medical Item`
     - Click to add it

3. **Configure MedicalItem Component:**

   **For Washing Tool (Step 2):**
   - **Item Name:** Type: `"Wash Cloth"` or `"Water Bottle"` (descriptive name)
   - **Item Tag:** Type exactly: `"WashCloth"` (must match Step 1's Required Tool Tag)
     - **OR** if you prefer: `"Water"` (but then change Step 1's tag to match)
   - **Can Be Grabbed:** ✅ Check this
   - **Return To Original Position:** ✅ Check this
   - **Destroy On Correct Drop:** ✅ Check this (tool disappears after use)

   **For Drying Tool (Step 3):**
   - **Item Name:** Type: `"Towel"` or `"Dry Cloth"`
   - **Item Tag:** Type exactly: `"Towel"` (must match Step 2's Required Tool Tag)
   - **Can Be Grabbed:** ✅ Check this
   - **Return To Original Position:** ✅ Check this
   - **Destroy On Correct Drop:** ✅ Check this

   **For Disinfecting Tool (Step 4):**
   - **Item Name:** Type: `"Antiseptic"` or `"Disinfectant"`
   - **Item Tag:** Type exactly: `"Antiseptic"` (must match Step 3's Required Tool Tag)
   - **Can Be Grabbed:** ✅ Check this
   - **Return To Original Position:** ✅ Check this
   - **Destroy On Correct Drop:** ✅ Check this

   **For Bandage Tool (Step 5):**
   - **Item Name:** Type: `"Bandage"` or `"Gauze"`
   - **Item Tag:** Type exactly: `"Bandage"` (must match Step 4's Required Tool Tag)
   - **Can Be Grabbed:** ✅ Check this
   - **Return To Original Position:** ✅ Check this
   - **Destroy On Correct Drop:** ✅ Check this

4. **Verify Tool Has Collider:**
   - Check if tool has a Collider component
   - If not, add one (Box Collider usually works)
   - The collider is needed for grabbing

5. **Verify Tool Has Rigidbody (Optional but Recommended):**
   - Check if tool has a Rigidbody component
   - If not, the MedicalItem script will add one automatically
   - If it does exist, make sure it's set to `Is Kinematic` initially

#### Step 3.3: Verify Tag Matching

**Critical:** The tool tags must match the step requirements exactly.

Create a checklist:
- [ ] Washing tool tag = Step 1's Required Tool Tag
- [ ] Drying tool tag = Step 2's Required Tool Tag
- [ ] Disinfecting tool tag = Step 3's Required Tool Tag
- [ ] Bandage tool tag = Step 4's Required Tool Tag

**Example:**
- If your washing tool has tag `"WashCloth"`, then Step 1 (Element 1) must have Required Tool Tag = `"WashCloth"`
- If your bandage tool has tag `"Bandage"`, then Step 4 (Element 4) must have Required Tool Tag = `"Bandage"`

---

### PART 4: UI Creation Guide

#### Step 4.1: Ensure You Have a Canvas

1. **Check for Canvas:**
   - In Hierarchy, look for `Canvas`
   - If it doesn't exist:
     - Right-click in Hierarchy
     - Select `UI` → `Canvas`
     - This creates a Canvas with an EventSystem

2. **Canvas Settings (Recommended):**
   - Select Canvas
   - In Inspector, find `Canvas` component
   - **Render Mode:** `Screen Space - Overlay` (default is fine)
   - **Canvas Scaler:**
     - **UI Scale Mode:** `Scale With Screen Size`
     - **Reference Resolution:** 1920 x 1080 (or your target resolution)

#### Step 4.2: Create Step Panels (5 Panels)

**For Each Step Panel (Repeat 5 times):**

1. **Create the Panel:**
   - Right-click on `Canvas` in Hierarchy
   - Select `UI` → `Panel`
   - Name it: `Step1Panel` (or Step2Panel, Step3Panel, etc.)

2. **Configure the Panel:**
   - Select the panel
   - In Inspector, find `Rect Transform`:
     - **Anchor Presets:** Hold Shift+Alt and click `Stretch-Stretch` (bottom-right option)
       - This makes it full-screen
     - **Left, Right, Top, Bottom:** All set to 0 (full screen)

3. **Add Content to Panel:**
   - **Add Text:**
     - Right-click on the panel
     - Select `UI` → `Text - TextMeshPro` (or `Text` if TMP not available)
     - Name it: `InstructionText`
     - Position it where you want
     - Type instructions like:
       - Step 1: "Tap on the injured knee to inspect it"
       - Step 2: "Use the wash cloth to clean the wound"
       - Step 3: "Use the towel to dry the wound"
       - Step 4: "Apply antiseptic to disinfect the wound"
       - Step 5: "Apply the bandage to cover the wound"
   - **Add Image (Optional):**
     - Right-click on the panel
     - Select `UI` → `Image`
     - Add an icon or illustration for the step

4. **Set Panel to Inactive:**
   - In Inspector, at the top, uncheck the checkbox next to the GameObject name
   - **OR** click the eye icon in Hierarchy
   - This hides the panel initially

5. **Assign to FirstAidGameManager:**
   - Select `FirstAidGameManager`
   - In Inspector, find `Treatment Steps`
   - Expand the appropriate Element (0-4)
   - Drag the panel into `Step Panel` field

**Repeat for all 5 steps:**
- Step1Panel → Element 0
- Step2Panel → Element 1
- Step3Panel → Element 2
- Step4Panel → Element 3
- Step5Panel → Element 4

#### Step 4.3: Create Wrong Tool Panel

1. **Create the Panel:**
   - Right-click on `Canvas`
   - Select `UI` → `Panel`
   - Name it: `WrongToolPanel`

2. **Make it Full Screen:**
   - Select `WrongToolPanel`
   - In `Rect Transform`, set Anchor Presets to `Stretch-Stretch` (Shift+Alt+Click)
   - Set Left, Right, Top, Bottom to 0

3. **Style the Panel:**
   - In Inspector, find `Image` component
   - **Color:** Set to semi-transparent red or your preferred color
   - **Alpha:** Set to 200-240 (semi-transparent)

4. **Add Warning Text:**
   - Right-click on `WrongToolPanel`
   - Select `UI` → `Text - TextMeshPro`
   - Name it: `WarningText`
   - Center it on screen
   - Type: `"WRONG TOOL!"` or `"That's not the right tool for this step!"`
   - Make text large and bold
   - Color: Red or white (visible on background)

5. **Set to Inactive:**
   - Uncheck the checkbox in Inspector (hide it initially)

6. **Assign to FirstAidGameManager:**
   - Select `FirstAidGameManager`
   - Drag `WrongToolPanel` into `Wrong Tool Panel` field

#### Step 4.4: Create Success Panel

1. **Create the Panel:**
   - Right-click on `Canvas`
   - Select `UI` → `Panel`
   - Name it: `SuccessPanel`

2. **Make it Full Screen:**
   - Set Anchor Presets to `Stretch-Stretch`
   - Set all margins to 0

3. **Style the Panel:**
   - **Image Color:** Semi-transparent green or your preferred success color
   - **Alpha:** 200-240

4. **Add Success Text:**
   - Add Text child: `"SUCCESS!"` or `"You successfully treated the injury!"`
   - Center it, make it large and bold
   - Color: Green or white

5. **Add Button (Optional):**
   - Right-click on `SuccessPanel`
   - Select `UI` → `Button - TextMeshPro`
   - Name it: `RestartButton`
   - Position it below the text
   - Change button text to: `"Play Again"` or `"Restart"`
   - Later, you can add a script to reset the game

6. **Set to Inactive:**
   - Uncheck the checkbox

7. **Assign to FirstAidGameManager:**
   - Drag `SuccessPanel` into `Success Panel` field

#### Step 4.5: Create Fail Panel

1. **Create the Panel:**
   - Right-click on `Canvas`
   - Select `UI` → `Panel`
   - Name it: `FailPanel`

2. **Make it Full Screen:**
   - Set Anchor Presets to `Stretch-Stretch`
   - Set all margins to 0

3. **Style the Panel:**
   - **Image Color:** Semi-transparent dark red or black
   - **Alpha:** 200-240

4. **Add Fail Text:**
   - Add Text child: `"GAME OVER"` or `"You made too many mistakes!"`
   - Center it, make it large and bold
   - Color: Red or white

5. **Add Button (Optional):**
   - Add a `RestartButton` similar to Success Panel

6. **Set to Inactive:**
   - Uncheck the checkbox

7. **Assign to FirstAidGameManager:**
   - Drag `FailPanel` into `Fail Panel` field

#### Step 4.6: Create Strikes Display System

1. **Create Strikes Parent:**
   - Right-click on `Canvas`
   - Select `Create Empty`
   - Name it: `StrikesParent`
   - Position it in top-right corner (or wherever you want strikes)
   - In `Rect Transform`:
     - Set Anchor to top-right
     - Position: X = -50, Y = -50 (adjust as needed)

2. **Create Strike X Prefab:**
   - Right-click on `Canvas`
   - Select `UI` → `Image`
   - Name it: `StrikeX`
   - **Image Component:**
     - **Source Image:** 
       - Click the circle icon
       - If you have an X sprite, select it
       - **OR** create a simple X:
         - You can use a Text component instead: `UI` → `Text - TextMeshPro`
         - Type: `"X"`
         - Font size: 48 or larger
         - Color: Red
   - **Rect Transform:**
     - Width: 50, Height: 50 (adjust as needed)
   - **Make it a Prefab:**
     - Drag `StrikeX` from Hierarchy into your `Prefabs` folder in Project window
     - This creates a prefab
     - Delete the original from Hierarchy (or keep it for testing)

3. **Assign to FirstAidGameManager:**
   - Select `FirstAidGameManager`
   - Drag `StrikesParent` into `Strikes Parent` field
   - Drag the `StrikeX` prefab into `Strike X Prefab` field

**Strikes Layout (Optional - for better visual):**
- You can add a `Horizontal Layout Group` component to `StrikesParent`
- This will automatically arrange X marks horizontally
- Set `Spacing` to 10 or 20

---

### PART 5: Texture/Material Setup

#### Step 5.1: Locate Your Textures

Your textures should be in the Project window. Common locations:
- `Assets/Textures/`
- `Assets/Characters/Characters lvl1/`
- `Assets/PlayGround Assets lvl1/`

Look for textures named:
- `Boy3ScrappedKnee`
- `Boy3ScrappedKneeCleanWet`
- `Boy3ScrappedKneeCleanDry`
- `Boy3ScrappedKneeDisinfected`
- `Boy3ScrappedKneeBandaged`

#### Step 5.2: Using Textures Directly

If you have textures (`.png`, `.jpg`, etc.):

1. **In FirstAidGameManager:**
   - For each Treatment Step, assign the texture to `Character Texture` field
   - The script will create a material from the texture automatically

2. **Texture Import Settings (Recommended):**
   - Select a texture in Project window
   - In Inspector, check:
     - **Texture Type:** `Default` or `Sprite` (depending on usage)
     - **Max Size:** 2048 or 4096 (depending on quality needed)
     - **Compression:** `None` or `High Quality` (for better quality)

#### Step 5.3: Using Materials Instead

If you have materials (`.mat` files) instead of textures:

1. **In FirstAidGameManager:**
   - Leave `Character Texture` empty
   - Assign the material to `Character Material` field instead

2. **Material Setup:**
   - Select the material in Project window
   - Ensure it has the correct texture assigned in its main texture slot
   - The material should be compatible with the character's shader

#### Step 5.4: Verify Texture Assignment

1. **Check Each Step:**
   - Go through each Treatment Step in FirstAidGameManager
   - Verify that each step has either:
     - A texture assigned to `Character Texture`, OR
     - A material assigned to `Character Material`
   - Don't leave both empty (the character won't change appearance)

2. **Test in Play Mode:**
   - Enter Play Mode
   - The character should start with the first texture (Step 0)
   - As you progress, the texture should change

---

### PART 6: PlayerController Setup (If Not Already Done)

#### Step 6.1: Locate Player Controller

The PlayerController should be attached to:
- The player character GameObject, OR
- The main camera, OR
- A separate controller GameObject

#### Step 6.2: Configure PlayerController

1. **Select the GameObject with PlayerController**

2. **Configure Settings:**
   - **Grab Distance:** Set to `3` or higher (how far player can grab items)
   - **Hold Point:**
     - If empty, the script will create one automatically
     - **OR** create manually:
       - Right-click on the player GameObject
       - Select `Create Empty`
       - Name it: `HoldPoint`
       - Position it where items should appear when held (e.g., in front of character)
       - Drag it into `Hold Point` field

3. **Verify It Works:**
   - Enter Play Mode
   - Click on a tool - it should be grabbed
   - Click again - it should be released

---

## Testing Procedures

### Initial Setup Test

1. **Enter Play Mode**
2. **Check Console:**
   - Open Console window (Window → General → Console)
   - Look for any errors (red text)
   - Fix any errors before continuing

3. **Verify Game Manager Initialized:**
   - Check that Step 1 panel is visible
   - Check that character has the first texture applied

### Step-by-Step Testing

#### Test 1: Step 1 (Tap Interaction)

1. **Verify Step 1 Panel is Visible:**
   - The first step panel should be showing
   - Character should have the initial injured texture

2. **Test Tap:**
   - Click on the injured knee area (where InjuryZone is)
   - **Expected Result:**
     - Step 1 panel disappears
     - Step 2 panel appears
     - Character texture changes to clean/wet texture
     - Console shows: "Injury tapped successfully!"

3. **If Tap Doesn't Work:**
   - Check InjuryZone has collider
   - Check `Allow Tap Interaction` is enabled
   - Check collider is positioned over knee
   - Check collider is large enough
   - Try clicking directly on the collider in Scene view

#### Test 2: Step 2 (Washing Tool)

1. **Verify Step 2 Panel is Visible**

2. **Test Correct Tool:**
   - Click on the washing tool to grab it
   - Drag it over the injury zone
   - Release (click again)
   - **Expected Result:**
     - Tool disappears
     - Step 2 panel disappears
     - Step 3 panel appears
     - Character texture changes to clean/dry texture
     - Console shows: "Correct tool [tool name] used!"

3. **Test Wrong Tool:**
   - Grab a different tool (not the washing tool)
   - Drop it on the injury
   - **Expected Result:**
     - Wrong Tool Panel appears (full screen)
     - Panel disappears after 2 seconds
     - Tool disappears
     - Strike X appears in top-right
     - Game continues (Step 2 panel still visible)
     - Console shows: "Wrong tool [tool name] used!"

#### Test 3: Remaining Steps

Repeat Test 2 for Steps 3, 4, and 5:
- Step 3: Use drying tool
- Step 4: Use disinfecting tool
- Step 5: Use bandage tool

#### Test 4: Win Condition

1. **Complete All Steps Correctly:**
   - Complete all 5 steps without mistakes
   - **Expected Result:**
     - After Step 5, Success Panel appears
     - All step panels are hidden
     - Console shows: "Game Won!"

#### Test 5: Lose Condition

1. **Make 3 Mistakes:**
   - Use wrong tool 3 times
   - **Expected Result:**
     - After 1st mistake: 1 strike X appears
     - After 2nd mistake: 2 strike X's appear
     - After 3rd mistake: 3 strike X's appear, Fail Panel appears
     - Game ends
     - Console shows: "Game Lost!"

#### Test 6: Edge Cases

1. **Drop Tool Outside Injury Zone:**
   - Grab a tool
   - Drop it away from the injury
   - **Expected:** Tool returns to original position

2. **Try to Use Tool on Wrong Step:**
   - Complete Step 1 (tap)
   - Try to use Step 5's tool (bandage) on Step 2
   - **Expected:** Wrong tool panel, strike added

3. **Try to Tap When Tool Required:**
   - On Step 2, try tapping instead of using tool
   - **Expected:** Nothing happens (tap only works on Step 1)

---

## Troubleshooting

### Problem: Tools Not Appearing When Grabbed

**Symptoms:** Tool doesn't move when clicked

**Solutions:**
1. Check tool has `MedicalItem` component
2. Check `Can Be Grabbed` is enabled
3. Check tool has a Collider
4. Check PlayerController is in the scene
5. Check `Grab Distance` is large enough
6. Check tool is not already grabbed
7. Check tool's layer is not ignored by camera culling

### Problem: Tools Not Disappearing After Use

**Symptoms:** Tool stays visible after correct use

**Solutions:**
1. Check `Destroy On Correct Drop` is enabled on MedicalItem
2. Check InjuryZone is properly calling TryDropItem
3. Check tool GameObject is being set inactive (check InjuryZone script)
4. Verify tool tag matches step requirement exactly

### Problem: Wrong Tool Panel Not Showing

**Symptoms:** Wrong tool used but no panel appears

**Solutions:**
1. Check Wrong Tool Panel is assigned in FirstAidGameManager
2. Check panel is initially inactive (should be)
3. Check panel is a child of Canvas
4. Check panel's RectTransform is set to full screen
5. Check Console for errors
6. Verify `Wrong Tool Panel Duration` is set (should be 2 seconds)

### Problem: Tapping Not Working

**Symptoms:** Can't tap on injury for Step 1

**Solutions:**
1. Check InjuryZone has a Collider
2. Check `Allow Tap Interaction` is enabled
3. Check Tap Collider is assigned
4. Check collider is positioned over knee
5. Check collider is large enough (try making it bigger)
6. Check collider is not blocked by other objects
7. Check you're not holding a tool when tapping
8. Verify Step 0 has `Requires Tap Only` checked
9. Check game is not ended
10. Try clicking directly on the collider in Scene view

### Problem: Texture Not Changing

**Symptoms:** Character appearance doesn't change between steps

**Solutions:**
1. Check Character Renderer is assigned correctly
2. Check textures are assigned to each step
3. Check texture names are correct
4. Check textures are imported correctly (not corrupted)
5. Check material supports texture changes
6. Verify renderer component is the one that displays the character
7. Try using materials instead of textures
8. Check Console for errors about texture assignment

### Problem: Game Not Progressing

**Symptoms:** Steps don't advance after correct action

**Solutions:**
1. Check tool tags match exactly (case-sensitive)
2. Check all step panels are assigned
3. Check game manager is not in processing state (check Console)
4. Verify `isProcessingAction` is not stuck (restart game)
5. Check Treatment Steps array has 5 elements
6. Verify each step has required fields filled
7. Check Console for errors

### Problem: Strikes Not Appearing

**Symptoms:** Wrong tool used but no strike X appears

**Solutions:**
1. Check Strikes Parent is assigned
2. Check Strike X Prefab is assigned
3. Check prefab is set up correctly (has Image or Text component)
4. Check Strikes Parent is visible and positioned correctly
5. Check prefab is not null
6. Verify strikes are being added (check Console for strike count)

### Problem: Success/Fail Panels Not Showing

**Symptoms:** Game ends but no panel appears

**Solutions:**
1. Check Success Panel is assigned
2. Check Fail Panel is assigned
3. Check panels are children of Canvas
4. Check panels are initially inactive
5. Check panels' RectTransform is set to full screen
6. Check Console for errors
7. Verify game is actually ending (check `gameEnded` flag)

### Problem: Tools Can't Be Grabbed

**Symptoms:** Clicking on tools does nothing

**Solutions:**
1. Check PlayerController is in scene
2. Check tool has MedicalItem component
3. Check `Can Be Grabbed` is enabled
4. Check tool has Collider
5. Check `Grab Distance` is sufficient
6. Check camera can see the tool
7. Check tool's layer is not ignored
8. Try increasing Grab Distance
9. Check for other scripts interfering

### Problem: Drag and Drop Not Working

**Symptoms:** Can grab tool but can't drop it on injury

**Solutions:**
1. Check InjuryZone has Collider (as trigger)
2. Check InjuryZone component is added
3. Check Game Manager is assigned to InjuryZone
4. Check tool tag matches step requirement
5. Check you're dropping on the injury zone (not just near it)
6. Try increasing collider size
7. Check Console for drop attempt messages
8. Verify InjuryZone is positioned correctly

---

## Common Mistakes

### Mistake 1: Tool Tags Don't Match

**Error:** Tool tag "Bandage" but step requires "bandage" (case mismatch)

**Fix:** Make tags match exactly, including capitalization

**Prevention:** Use a consistent naming convention (e.g., always capitalize first letter)

### Mistake 2: Forgot to Check "Requires Tap Only"

**Error:** Step 1 doesn't work because tap interaction is disabled

**Fix:** Check `Requires Tap Only` for Element 0 (Step 1)

**Prevention:** Double-check Step 1 configuration

### Mistake 3: Collider Not Set as Trigger

**Error:** Tools can't be dropped on injury zone

**Fix:** Check `Is Trigger` on InjuryZone's collider

**Prevention:** Always verify collider settings

### Mistake 4: Panels Not Initially Inactive

**Error:** All panels show at once, game looks broken

**Fix:** Set all panels to inactive initially (except maybe first step panel for testing)

**Prevention:** Create a checklist: all panels must be inactive except current step

### Mistake 5: Wrong Renderer Assigned

**Error:** Texture changes but character doesn't update

**Fix:** Find the correct renderer (usually on a child object, not the main GameObject)

**Prevention:** Test texture assignment in Play Mode immediately

### Mistake 6: Forgot to Assign References

**Error:** Null reference exceptions in Console

**Fix:** Go through FirstAidGameManager and assign all required references

**Prevention:** Use the testing checklist to verify all assignments

### Mistake 7: Step Panels in Wrong Order

**Error:** Wrong panel shows for each step

**Fix:** Verify panel assignments match step order (Step1Panel → Element 0, etc.)

**Prevention:** Name panels clearly and double-check assignments

### Mistake 8: Texture Names Don't Match

**Error:** Can't find textures, or wrong texture applied

**Fix:** Verify texture names match exactly what's in Project window

**Prevention:** Use consistent naming: `Boy3ScrappedKnee[State]`

---

## Quick Reference Checklist

Use this checklist when setting up:

### FirstAidGameManager
- [ ] GameObject created and component added
- [ ] Injured Character assigned
- [ ] Character Renderer assigned (correct one!)
- [ ] Treatment Steps array size = 5
- [ ] Element 0: Step name, texture, tap only checked, no tool tag
- [ ] Element 1: Step name, texture, tool tag matches washing tool
- [ ] Element 2: Step name, texture, tool tag matches drying tool
- [ ] Element 3: Step name, texture, tool tag matches disinfecting tool
- [ ] Element 4: Step name, texture, tool tag matches bandage tool
- [ ] Wrong Tool Panel assigned
- [ ] Success Panel assigned
- [ ] Fail Panel assigned
- [ ] Strikes Parent assigned
- [ ] Strike X Prefab assigned

### InjuryZone
- [ ] GameObject created at knee location
- [ ] Collider added and set as trigger
- [ ] Collider sized appropriately
- [ ] InjuryZone component added
- [ ] Game Manager assigned
- [ ] Tap Collider assigned
- [ ] Allow Tap Interaction checked
- [ ] Single Item Only checked
- [ ] Snap To Center checked

### Tools (Each Tool)
- [ ] MedicalItem component added
- [ ] Item Name set
- [ ] Item Tag matches corresponding step requirement
- [ ] Can Be Grabbed checked
- [ ] Return To Original Position checked
- [ ] Destroy On Correct Drop checked
- [ ] Collider present
- [ ] Tag verified to match step (case-sensitive)

### UI
- [ ] Canvas exists in scene
- [ ] Step1Panel created, full screen, inactive, assigned to Element 0
- [ ] Step2Panel created, full screen, inactive, assigned to Element 1
- [ ] Step3Panel created, full screen, inactive, assigned to Element 2
- [ ] Step4Panel created, full screen, inactive, assigned to Element 3
- [ ] Step5Panel created, full screen, inactive, assigned to Element 4
- [ ] WrongToolPanel created, full screen, inactive, assigned
- [ ] SuccessPanel created, full screen, inactive, assigned
- [ ] FailPanel created, full screen, inactive, assigned
- [ ] StrikesParent created, positioned, assigned
- [ ] StrikeX prefab created, assigned

### Textures/Materials
- [ ] All 5 textures/materials located
- [ ] Textures assigned to correct steps
- [ ] Textures imported correctly

### PlayerController
- [ ] Component added to appropriate GameObject
- [ ] Grab Distance set
- [ ] Hold Point created/assigned

### Testing
- [ ] Enter Play Mode - no errors in Console
- [ ] Step 1 panel visible
- [ ] Tap on knee works → Step 2 appears
- [ ] Correct tool on Step 2 works → Step 3 appears
- [ ] Wrong tool shows panel and strike
- [ ] All steps complete → Success panel
- [ ] 3 wrong tools → Fail panel

---

## Additional Tips

1. **Organization:** Keep all UI elements organized in Hierarchy (use empty GameObjects as folders)

2. **Naming Convention:** Use consistent naming:
   - Tools: `WashCloth`, `Towel`, `Antiseptic`, `Bandage`
   - Panels: `Step1Panel`, `Step2Panel`, etc.
   - Tags: Match tool names exactly

3. **Testing:** Test frequently during setup - don't wait until everything is done

4. **Backup:** Save your scene frequently, especially before making major changes

5. **Console:** Keep Console window open to catch errors early

6. **Documentation:** Write down your tool tags and step requirements to avoid mismatches

7. **Visual Feedback:** Make sure panels are visible and readable (good contrast, large text)

8. **Collider Sizing:** Make injury zone collider large enough to be easily clickable, but not so large it overlaps other objects

---

## Support

If you encounter issues not covered in this guide:

1. Check the Console for error messages
2. Verify all references are assigned (no null references)
3. Test each component individually
4. Compare your setup with the checklist
5. Review the script comments in the code files

Good luck with your first aid game setup!
