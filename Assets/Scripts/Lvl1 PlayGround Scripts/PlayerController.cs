using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Grab Settings")]
    [Tooltip("Maximum distance to grab items")]
    public float grabDistance = 3f;
    
    [Tooltip("Layer mask for items that can be grabbed")]
    public LayerMask grabableLayer;
    
    [Tooltip("Transform where grabbed items will be held")]
    public Transform holdPoint;
    
    [Header("Visual Feedback")]
    [Tooltip("Cursor texture when hovering over grabable items")]
    public Texture2D grabCursor;
    
    private GameObject heldItem;
    private Camera playerCamera;
    private bool isGrabbing = false;
    private bool isMousePressed = false;
    private bool wasMousePressedLastFrame = false;
    
    void Start()
    {
        // Find main camera if not assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                playerCamera = FindObjectOfType<Camera>();
            }
        }
        
        // Create hold point if not assigned
        if (holdPoint == null)
        {
            GameObject holdPointObj = new GameObject("HoldPoint");
            holdPointObj.transform.SetParent(transform);
            holdPointObj.transform.localPosition = new Vector3(0, 0.5f, 1f); // Adjust based on arm position
            holdPoint = holdPointObj.transform;
        }
    }
    
    void Update()
    {
        HandleInput();
        UpdateHeldItemPosition();
    }
    
    void HandleInput()
    {
        // Check for grab input (left mouse button or touch) - New Input System
        Mouse mouse = Mouse.current;
        Touchscreen touchscreen = Touchscreen.current;
        
        bool mousePressed = false;
        bool mouseReleased = false;
        
        // Check mouse input
        if (mouse != null)
        {
            isMousePressed = mouse.leftButton.isPressed;
            
            // Check if mouse was just pressed (start of drag)
            if (mouse.leftButton.wasPressedThisFrame && !wasMousePressedLastFrame)
            {
                mousePressed = true;
            }
            
            // Check if mouse was just released (end of drag)
            if (!mouse.leftButton.isPressed && wasMousePressedLastFrame)
            {
                mouseReleased = true;
            }
            
            // Alternative: Release on right click
            if (mouse.rightButton.wasPressedThisFrame && heldItem != null)
            {
                ReleaseItem();
            }
        }
        
        // Check touch input
        if (touchscreen != null)
        {
            bool touchActive = touchscreen.primaryTouch.isInProgress;
            
            if (touchscreen.primaryTouch.press.wasPressedThisFrame)
            {
                mousePressed = true;
                isMousePressed = true;
            }
            
            if (!touchActive && isMousePressed)
            {
                mouseReleased = true;
                isMousePressed = false;
            }
        }
        
        // Handle input based on state
        if (mousePressed && heldItem == null)
        {
            // Press to grab item
            TryGrabItem();
        }
        else if (mouseReleased && heldItem != null)
        {
            // Release to drop item
            ReleaseItem();
        }
        
        // Update last frame state
        wasMousePressedLastFrame = isMousePressed;
    }
    
    void TryGrabItem()
    {
        // Get mouse or touch position - New Input System
        Vector2 screenPosition = GetScreenPosition();
        Ray ray = playerCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // Check if clicking on MedicalKit to open/close it
            MedicalKit medicalKit = hitObject.GetComponent<MedicalKit>();
            if (medicalKit != null)
            {
                medicalKit.Interact();
                return;
            }
            
            // Check if the object has a MedicalItem component
            MedicalItem item = hitObject.GetComponent<MedicalItem>();
            if (item != null && item.CanBeGrabbed())
            {
                GrabItem(item);
            }
            // Also check parent for MedicalItem (in case collider is on child)
            else if (hitObject.transform.parent != null)
            {
                // Check parent for MedicalKit
                medicalKit = hitObject.transform.parent.GetComponent<MedicalKit>();
                if (medicalKit != null)
                {
                    medicalKit.Interact();
                    return;
                }
                
                // Check parent for MedicalItem
                item = hitObject.transform.parent.GetComponent<MedicalItem>();
                if (item != null && item.CanBeGrabbed())
                {
                    GrabItem(item);
                }
            }
        }
    }
    
    void GrabItem(MedicalItem item)
    {
        if (item == null) return;
        
        heldItem = item.gameObject;
        isGrabbing = true;
        
        // Notify the item it's being grabbed
        item.OnGrabbed(holdPoint);
        
        Debug.Log($"Grabbed item: {item.itemName} (Tag: {item.itemTag})");
    }
    
    void ReleaseItem()
    {
        if (heldItem == null) return;
        
        MedicalItem item = heldItem.GetComponent<MedicalItem>();
        if (item != null)
        {
            // Check if we're over a valid drop zone
            // Get mouse or touch position - New Input System
            Vector2 screenPosition = GetScreenPosition();
            Ray ray = playerCamera.ScreenPointToRay(screenPosition);
            
            // Use a much longer distance for drop detection to ensure we hit the injury zone
            float dropDistance = 100f;
            
            // Cast ray and check all hits
            RaycastHit[] hits = Physics.RaycastAll(ray, dropDistance);
            
            // Sort hits by distance to get the closest one first
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            
            // Check all hits for InjuryZone (in case item is blocking the raycast)
            foreach (RaycastHit hit in hits)
            {
                // Skip the item itself
                if (hit.collider.gameObject == heldItem || hit.collider.transform.IsChildOf(heldItem.transform))
                    continue;
                
                // Check for InjuryZone
                InjuryZone injuryZone = hit.collider.GetComponent<InjuryZone>();
                if (injuryZone != null)
                {
                    Debug.Log($"Raycast hit InjuryZone at distance {hit.distance}. Attempting to drop item.");
                    // Try to drop in injury zone
                    if (injuryZone.TryDropItem(item))
                    {
                        // InjuryZone handles the drop logic internally
                        heldItem = null;
                        isGrabbing = false;
                        return;
                    }
                    else
                    {
                        Debug.Log("TryDropItem returned false - tool may be wrong or step doesn't accept tools.");
                    }
                    break; // Found the injury zone, no need to check further
                }
            }
            
            Debug.Log("Item released but not dropped on injury zone. Returning to original position.");
            // Drop item at current position (return to original position)
            item.OnReleased();
        }
        
        heldItem = null;
        isGrabbing = false;
    }
    
    void UpdateHeldItemPosition()
    {
        if (heldItem != null && playerCamera != null)
        {
            // Get mouse/touch position in world space
            Vector2 screenPosition = GetScreenPosition();
            Ray ray = playerCamera.ScreenPointToRay(screenPosition);
            
            // Position item way closer to camera so it appears on top/in front of the character
            // Use a very close distance to make item appear in front and on top of the knee
            float distanceFromCamera = 0.4f; // Very close to camera = appears on top/in front
            
            // Calculate target position along the ray
            Vector3 targetPosition = ray.GetPoint(distanceFromCamera);
            
            // Smoothly move item to follow cursor
            heldItem.transform.position = Vector3.Lerp(
                heldItem.transform.position,
                targetPosition,
                Time.deltaTime * 20f
            );
            
            // Rotate item to face camera (so it's always visible)
            Vector3 directionToCamera = playerCamera.transform.position - heldItem.transform.position;
            if (directionToCamera != Vector3.zero)
            {
                // Keep item rotated to face camera, but maintain upright orientation
                Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);
                heldItem.transform.rotation = Quaternion.Lerp(
                    heldItem.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 10f
                );
            }
        }
    }
    
    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
    
    public GameObject GetHeldItem()
    {
        return heldItem;
    }
    
    /// <summary>
    /// Gets the current screen position from mouse or touch input (New Input System)
    /// </summary>
    Vector2 GetScreenPosition()
    {
        Mouse mouse = Mouse.current;
        Touchscreen touchscreen = Touchscreen.current;
        
        if (mouse != null)
        {
            return mouse.position.ReadValue();
        }
        else if (touchscreen != null && touchscreen.primaryTouch.isInProgress)
        {
            return touchscreen.primaryTouch.position.ReadValue();
        }
        
        // Fallback to screen center if no input device
        return new Vector2(Screen.width / 2f, Screen.height / 2f);
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize grab distance
        Gizmos.color = Color.yellow;
        if (playerCamera != null)
        {
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * grabDistance);
        }
        
        // Visualize hold point
        if (holdPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPoint.position, 0.2f);
        }
    }
}

