using UnityEngine;
using UnityEngine.Events;

public class InteractableItem : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;         // Drag PlayerArmature here
    public float distance = 3.0f;    // How close to be?

    [Header("What happens when pressed?")]
    public UnityEvent onPressed;     // We will link this to the Manager later

    private void Update()
    {
        if (player == null) return;

        // Check distance
        if (Vector3.Distance(transform.position, player.position) < distance)
        {
            // Check input
            if (Input.GetKeyDown(KeyCode.E))
            {
                onPressed.Invoke(); // Trigger whatever is hooked up!
            }
        }
    }

    // Draw the yellow circle to help you see the zone
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}