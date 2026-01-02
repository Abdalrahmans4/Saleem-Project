using UnityEngine;
using UnityEngine.UI;

public class QuizAnswerButton : MonoBehaviour
{
    public bool isCorrect = false; // Check this for the Right Answer
    private Button myButton;
    private QuizController controller;

    void Start()
    {
        myButton = GetComponent<Button>();
        // Find the manager automatically
        controller = Object.FindFirstObjectByType<QuizController>();

        myButton.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        if (controller != null)
        {
            // Send this button (so we can grey it out) and the answer status
            controller.ProcessAnswer(isCorrect, transform.parent.gameObject);
        }
    }
}