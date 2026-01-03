using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InjuryZone : MonoBehaviour
{
    [Header("Injury Settings")]
    [Tooltip("Reference to the First Aid Game Manager")]
    public FirstAidGameManager gameManager;
    
    [Tooltip("Collider for tap detection (should cover the knee area)")]
    public Collider tapCollider;
    
    [Tooltip("Can player tap on this injury?")]
    public bool allowTapInteraction = true;
    
    [Header("Drop Zone Settings")]
    [Tooltip("Should items snap to center when dropped?")]
    public bool snapToCenter = true;
    
    private Camera playerCamera;
    private Collider zoneCollider;
    private List<MedicalItem> droppedItems = new List<MedicalItem>();
    
    void Start()
    {
        // Find camera if not assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                playerCamera = FindObjectOfType<Camera>();
            }
        }
        
        // Find game manager if not assigned
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<FirstAidGameManager>();
        }
        
        // Setup zone collider
        zoneCollider = GetComponent<Collider>();
        if (zoneCollider == null)
        {
            // Add a box collider if none exists
            zoneCollider = gameObject.AddComponent<BoxCollider>();
        }
        
        // Ensure zone collider is a trigger for drop detection
        zoneCollider.isTrigger = true;
        
        // Setup tap collider if not assigned
        if (tapCollider == null)
        {
            tapCollider = zoneCollider;
        }
    }
    
    void Update()
    {
        // Handle tap input for steps that require tapping
        // Only check on mouse down, and only if not holding an item
        if (allowTapInteraction)
        {
            Mouse mouse = Mouse.current;
            Touchscreen touchscreen = Touchscreen.current;
            
            bool inputPressed = false;
            
            // Check mouse input
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                inputPressed = true;
            }
            // Check touch input
            else if (touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
            {
                inputPressed = true;
            }
            
            if (inputPressed)
            {
                // Check if player is holding an item first
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController != null && playerController.IsHoldingItem())
                {
                    // Player is holding an item, don't process tap (will be handled by drag-drop)
                    return;
                }
                
                CheckTapOnInjury();
            }
        }
    }
    
    void CheckTapOnInjury()
    {
        if (gameManager == null || gameManager.IsGameEnded())
            return;
        
        // Get mouse or touch position - New Input System
        Vector2 screenPosition = GetScreenPosition();
        
        // Check if we're clicking on the injury area
        Ray ray = playerCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        // Use a longer distance for tap detection
        float tapDistance = 100f;
        
        // Check if clicking on this injury zone
        // Try the tap collider first, then the zone collider
        Collider colToCheck = tapCollider != null ? tapCollider : GetComponent<Collider>();
        
        if (colToCheck != null)
        {
            // Use Physics.Raycast to check all colliders, not just the specific one
            if (Physics.Raycast(ray, out hit, tapDistance))
            {
                // Check if we hit this injury zone or its children
                if (hit.collider == colToCheck || hit.collider.transform.IsChildOf(transform))
                {
                    // Make sure we didn't click on a tool or medical kit
                    if (hit.collider.GetComponent<MedicalItem>() == null && 
                        hit.collider.GetComponent<MedicalKit>() == null)
                    {
                        // Try to process tap
                        if (gameManager.TryTapInjury())
                        {
                            Debug.Log("Injury tapped successfully!");
                        }
                    }
                }
            }
        }
    }
    
    public bool TryDropItem(MedicalItem item)
    {
        if (item == null || gameManager == null || gameManager.IsGameEnded())
            return false;
        
        // Get current step data
        FirstAidGameManager.TreatmentStep currentStep = gameManager.GetCurrentStepData();
        if (currentStep == null)
            return false;
        
        // Check if this step requires a tool
        if (currentStep.requiresTapOnly)
        {
            // This step doesn't accept tools, only taps
            Debug.Log("This step requires tapping, not a tool!");
            return false;
        }
        
        // Check if tool is correct for current step
        bool isCorrectTool = gameManager.TryUseTool(item.itemTag);
        
        if (isCorrectTool)
        {
            // Correct tool! Process the drop
            // Add item to dropped items list
            droppedItems.Add(item);
            
            // Position item in zone if snap to center is enabled
            if (snapToCenter)
            {
                item.transform.position = transform.position;
                item.transform.rotation = transform.rotation;
            }
            
            // Notify item it was dropped in zone
            item.OnDroppedInZone(transform);
            
            // Hide the tool after successful use
            item.gameObject.SetActive(false);
            Debug.Log($"Correct tool {item.itemName} used! Moving to next step.");
            
            return true;
        }
        else
        {
            // Wrong tool - game manager will handle the strike
            // Hide the tool
            item.gameObject.SetActive(false);
            Debug.Log($"Wrong tool {item.itemName} used! Strike added.");
            return false;
        }
    }
    
    public void RemoveItem(MedicalItem item)
    {
        if (droppedItems.Contains(item))
        {
            droppedItems.Remove(item);
        }
    }
    
    public bool HasItem()
    {
        return droppedItems.Count > 0;
    }
    
    public List<MedicalItem> GetDroppedItems()
    {
        return new List<MedicalItem>(droppedItems);
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
}

