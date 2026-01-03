using UnityEngine;

public class GeneralWinHandler : MonoBehaviour
{
    [Header("Panels")]
    public GameObject successPanel;
    public GameObject failPanel;

    [Header("Keys")]
    // Make sure these match what is in QuizController!
    public string successKey = "Level1Complete";
    public string failKey = "Level1Failed";

    void Start()
    {
        // 1. Check if we Failed
        if (PlayerPrefs.GetInt(failKey) == 1)
        {
            if (failPanel != null) failPanel.SetActive(true);

            // Clear the key so it doesn't pop up next time we restart
            PlayerPrefs.DeleteKey(failKey);
        }
        // 2. Check if we Succeeded
        else if (PlayerPrefs.GetInt(successKey) == 1)
        {
            if (successPanel != null) successPanel.SetActive(true);

            // Clear the key
            PlayerPrefs.DeleteKey(successKey);
        }
    }
}
