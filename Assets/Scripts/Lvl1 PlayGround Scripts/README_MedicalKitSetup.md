# Medical Kit Interaction System Setup Guide

This guide explains how to set up the medical kit interaction system for SaleemForUnity.

## Scripts Overview

1. **PlayerController.cs** - Handles player input for grabbing and releasing items
2. **MedicalItem.cs** - Script for individual medical items (bandages, scissors, etc.)
3. **MedicalKit.cs** - Script for the medical kit container
4. **DragDropZone.cs** - Script for drop zones where items can be placed

## Setup Instructions

### Step 1: Setup SaleemForUnity Character

1. Select the `SaleemForUnity` GameObject in your scene
2. Add the `PlayerController` component to it
3. In the Inspector, configure:
   - **Grab Distance**: How far the player can grab items (default: 3)
   - **Hold Point**: Create an empty GameObject as a child of SaleemForUnity and assign it here
     - Position it where the hands/arms would hold items (e.g., forward and slightly up)
     - Example position: (0, 1.5, 1) relative to character

### Step 2: Setup Medical Kit

1. Select the `Medical_Kit` GameObject in your scene
2. Add the `MedicalKit` component to it
3. Configure:
   - **Kit Items**: Drag all child items from the medical kit into this list
   - **Spawn Items On Open**: Set to false if items already exist in the scene
   - **Kit Animator**: (Optional) Assign if you have an animator for opening/closing

### Step 3: Setup Individual Medical Items

For each item inside the Medical_Kit:

1. Select the item GameObject
2. Add the `MedicalItem` component
3. Configure:
   - **Item Name**: Give it a descriptive name (e.g., "Bandage", "Scissors", "Antiseptic")
   - **Item Tag**: Assign a unique tag (e.g., "Bandage", "Scissors", "Antiseptic")
     - **Important**: Create these tags in Unity: Edit > Project Settings > Tags and Layers
   - **Can Be Grabbed**: Should be true
   - **Return To Original Position**: Set to true if items should return when released incorrectly
   - **Destroy On Correct Drop**: Set to true if items should disappear when placed correctly

4. Ensure each item has:
   - A Collider (BoxCollider, SphereCollider, etc.)
   - A Rigidbody (will be added automatically if missing)

### Step 4: Create Tags

1. Go to Edit > Project Settings > Tags and Layers
2. Add the following tags (or customize as needed):
   - Bandage
   - Scissors
   - Antiseptic
   - Gauze
   - Tweezers
   - Stethoscope
   - (Add more as needed for your items)

### Step 5: Setup Drop Zones

1. Create empty GameObjects where items should be dropped
2. Add the `DragDropZone` component to each
3. Configure:
   - **Accepted Item Tags**: Add the tags of items that can be dropped here
     - Example: For a "Wound" zone, add "Bandage" and "Antiseptic" tags
   - **Single Item Only**: Set to true if only one item should be in this zone
   - **Snap To Center**: Set to true if items should snap to zone center
4. Add a Collider (set as Trigger) to define the drop area
5. Optionally add visual materials for feedback

## Usage

### Player Controls:
- **Left Click**: Grab/Release items
- **Right Click**: Release held item

### How It Works:
1. Player clicks on a medical item to grab it
2. Item follows the hold point position
3. Player clicks again to drop the item
4. If dropped over a valid drop zone, item snaps to zone
5. If dropped elsewhere, item falls or returns to original position (based on settings)

## Tips

- Adjust the Hold Point position to match where the character's hands would be
- Use different tags for different item types to enable proper sorting
- Create visual feedback materials for drop zones to guide the player
- Test grab distance to ensure comfortable interaction range
- Consider adding UI hints or instructions for the player

## Future Enhancements

When you add animations:
- Hook up animation triggers in the `PlayerController.OnGrab()` and `OnRelease()` methods
- Add animation events to MedicalItem for grab/release animations
- Use the MedicalKit animator for opening/closing animations

