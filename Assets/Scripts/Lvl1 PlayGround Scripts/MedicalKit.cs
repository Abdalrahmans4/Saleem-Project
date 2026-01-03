using UnityEngine;
using System.Collections.Generic;

public class MedicalKit : MonoBehaviour
{
    [Header("Kit Settings")]
    [Tooltip("Items contained in this medical kit")]
    public List<GameObject> kitItems = new List<GameObject>();
    
    [Tooltip("Should items be spawned when kit is opened?")]
    public bool spawnItemsOnOpen = false;
    
    [Tooltip("Prefabs to spawn when kit opens (if spawnItemsOnOpen is true)")]
    public GameObject[] itemPrefabs;
    
    [Header("Visual Settings")]
    [Tooltip("Animation or effect when kit opens")]
    public Animator kitAnimator;
    
    [Tooltip("Animation trigger name for opening")]
    public string openAnimationTrigger = "Open";
    
    [Tooltip("Animation trigger name for closing")]
    public string closeAnimationTrigger = "Close";
    
    private bool isOpen = false;
    private List<MedicalItem> medicalItems = new List<MedicalItem>();
    
    void Start()
    {
        // Find all MedicalItem components in children
        medicalItems.AddRange(GetComponentsInChildren<MedicalItem>());
        
        // If spawnItemsOnOpen is enabled, initially hide items
        if (spawnItemsOnOpen)
        {
            foreach (var item in medicalItems)
            {
                if (item != null)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
        
        // Initialize kit items list if empty
        if (kitItems.Count == 0)
        {
            foreach (var item in medicalItems)
            {
                if (item != null)
                {
                    kitItems.Add(item.gameObject);
                }
            }
        }
    }
    
    public void OpenKit()
    {
        if (isOpen) return;
        
        isOpen = true;
        
        // Play open animation
        if (kitAnimator != null && !string.IsNullOrEmpty(openAnimationTrigger))
        {
            kitAnimator.SetTrigger(openAnimationTrigger);
        }
        
        // Spawn items if configured
        if (spawnItemsOnOpen && itemPrefabs != null && itemPrefabs.Length > 0)
        {
            SpawnItems();
        }
        else
        {
            // Activate existing items
            foreach (var item in medicalItems)
            {
                if (item != null)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
        
        Debug.Log("Medical Kit Opened!");
    }
    
    public void CloseKit()
    {
        if (!isOpen) return;
        
        isOpen = false;
        
        // Play close animation
        if (kitAnimator != null && !string.IsNullOrEmpty(closeAnimationTrigger))
        {
            kitAnimator.SetTrigger(closeAnimationTrigger);
        }
        
        Debug.Log("Medical Kit Closed!");
    }
    
    void SpawnItems()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return;
        
        // Clear existing items
        medicalItems.Clear();
        
        // Spawn each prefab
        foreach (var prefab in itemPrefabs)
        {
            if (prefab != null)
            {
                GameObject spawnedItem = Instantiate(prefab, transform);
                spawnedItem.transform.localPosition = Vector3.zero;
                
                MedicalItem item = spawnedItem.GetComponent<MedicalItem>();
                if (item != null)
                {
                    medicalItems.Add(item);
                }
            }
        }
    }
    
    public bool IsOpen()
    {
        return isOpen;
    }
    
    public List<MedicalItem> GetItems()
    {
        return medicalItems;
    }
    
    void OnMouseDown()
    {
        // Toggle kit open/close on click
        if (isOpen)
        {
            CloseKit();
        }
        else
        {
            OpenKit();
        }
    }
    
    // Method to be called by player controller when interacting
    public void Interact()
    {
        if (isOpen)
        {
            CloseKit();
        }
        else
        {
            OpenKit();
        }
    }
}

