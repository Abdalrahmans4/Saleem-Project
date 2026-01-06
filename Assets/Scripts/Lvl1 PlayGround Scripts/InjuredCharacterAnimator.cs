using UnityEngine;
using System.Collections;

public class InjuredCharacterAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("The Animator component on the injured character")]
    public Animator characterAnimator;
    
    [Tooltip("Name of the fallen/idle animation state (e.g., 'Fallen Idle', 'Idle Fallen')")]
    public string fallenIdleStateName = "Fallen Idle";
    
    [Tooltip("Should the animation play on start?")]
    public bool playOnStart = true;
    
    [Tooltip("Animation speed multiplier (1 = normal speed)")]
    public float animationSpeed = 1f;
    
    private void Start()
    {
        // Find animator if not assigned
        if (characterAnimator == null)
        {
            characterAnimator = GetComponent<Animator>();
            if (characterAnimator == null)
            {
                characterAnimator = GetComponentInChildren<Animator>();
            }
        }
        
        // Play fallen idle animation on start
        if (playOnStart && characterAnimator != null)
        {
            PlayFallenIdle();
        }
    }
    
    /// <summary>
    /// Plays the fallen idle animation
    /// </summary>
    public void PlayFallenIdle()
    {
        if (characterAnimator == null)
        {
            Debug.LogWarning("InjuredCharacterAnimator: Animator not found!");
            return;
        }
        
        // Check if animator is enabled
        if (!characterAnimator.enabled)
        {
            Debug.LogWarning("InjuredCharacterAnimator: Animator is disabled! Enabling it...");
            characterAnimator.enabled = true;
        }
        
        // Check if animator has a controller
        if (characterAnimator.runtimeAnimatorController == null)
        {
            Debug.LogError("InjuredCharacterAnimator: Animator Controller is not assigned! Please assign a controller in the Animator component.");
            return;
        }
        
        // Set animation speed
        characterAnimator.speed = animationSpeed;
        
        // Play the animation
        if (!string.IsNullOrEmpty(fallenIdleStateName))
        {
            // Try multiple methods to ensure animation plays
            // Method 1: Play by state name (most common)
            characterAnimator.Play(fallenIdleStateName, 0, 0f);
            
            // Force update to ensure it plays
            characterAnimator.Update(0f);
            
            // Verify the state is playing
            StartCoroutine(VerifyAnimationPlaying());
            
            Debug.Log($"InjuredCharacterAnimator: Attempting to play animation '{fallenIdleStateName}'");
        }
        else
        {
            Debug.LogWarning("InjuredCharacterAnimator: Fallen Idle State Name is not set!");
        }
    }
    
    /// <summary>
    /// Verifies that the animation is actually playing
    /// </summary>
    private System.Collections.IEnumerator VerifyAnimationPlaying()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (characterAnimator != null)
        {
            AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.IsName(fallenIdleStateName))
            {
                Debug.Log($"InjuredCharacterAnimator: Successfully playing '{fallenIdleStateName}' animation");
            }
            else
            {
                Debug.LogWarning($"InjuredCharacterAnimator: Animation state mismatch! Expected '{fallenIdleStateName}', but current state is '{stateInfo.fullPathHash}' or '{stateInfo.shortNameHash}'. " +
                    $"Please verify:\n" +
                    $"1. The state name '{fallenIdleStateName}' exists in your Animator Controller\n" +
                    $"2. The animation clip is assigned to the state\n" +
                    $"3. The state is set as the default state\n" +
                    $"4. The Avatar is properly configured");
            }
        }
    }
    
    /// <summary>
    /// Sets the animation speed
    /// </summary>
    public void SetAnimationSpeed(float speed)
    {
        if (characterAnimator != null)
        {
            characterAnimator.speed = speed;
        }
    }
    
    /// <summary>
    /// Gets the current animator component
    /// </summary>
    public Animator GetAnimator()
    {
        return characterAnimator;
    }
    
    /// <summary>
    /// Checks if the animator is playing the fallen idle animation
    /// </summary>
    public bool IsPlayingFallenIdle()
    {
        if (characterAnimator == null || string.IsNullOrEmpty(fallenIdleStateName))
            return false;
        
        AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(fallenIdleStateName);
    }
}

