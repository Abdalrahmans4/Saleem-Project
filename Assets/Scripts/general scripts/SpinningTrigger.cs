using UnityEngine;

public class SpinningTrigger : MonoBehaviour
{
    public BurnLevelManager manager;
    private bool playerInRange = false;

    void Update()
    {
        // Rotate Effect
        transform.Rotate(0, 100 * Time.deltaTime, 0);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            manager.StartMiniGameSequence();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}