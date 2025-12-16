using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizController : MonoBehaviour
{
    [Header("TESTING MODE")]
    public bool testSequenceMode = true; // Set to TRUE to play Level 1 -> 2 -> 3 automatically

    [Header("Level 1 Configuration")]
    public GameObject level1Parent;      // The parent holding Q1-Q5 for Level 1
    public GameObject[] level1Questions; // Drag Q1Panel, Q2Panel... here

    [Header("Level 2 Configuration")]
    public GameObject level2Parent;
    public GameObject[] level2Questions;

    [Header("Level 3 Configuration")]
    public GameObject level3Parent;
    public GameObject[] level3Questions;

    [Header("Feedback Images")]
    public GameObject correctImage; // The Green Checkmark Image
    public GameObject wrongImage;   // The Red X Image

    private GameObject[] currentQuestionSet; // This will hold the active list
    private int questionIndex = 0;
    private int score = 0;
    private bool isProcessing = false; // Prevents double clicking

    // Track which level is currently active so we know what comes next
    private int currentLevelID = 1;

    void Start()
    {
        int levelToLoad = 1;

        if (testSequenceMode)
        {
            // Force start at Level 1 for testing
            levelToLoad = 1;
        }
        else
        {
            // Load normally based on game progress
            levelToLoad = PlayerPrefs.GetInt("QuizToLoad", 1);
        }

        StartQuizLevel(levelToLoad);
    }

    // New helper function to setup the level cleanly
    void StartQuizLevel(int levelID)
    {
        currentLevelID = levelID;
        questionIndex = 0;
        score = 0; // Reset score for the new level

        // 1. Reset all parents first
        level1Parent.SetActive(false);
        level2Parent.SetActive(false);
        level3Parent.SetActive(false);

        // 2. Activate the correct parent and assign questions
        if (levelID == 1)
        {
            level1Parent.SetActive(true);
            currentQuestionSet = level1Questions;
        }
        else if (levelID == 2)
        {
            level2Parent.SetActive(true);
            currentQuestionSet = level2Questions;
        }
        else
        {
            level3Parent.SetActive(true);
            currentQuestionSet = level3Questions;
        }

        // 3. Setup the first question
        correctImage.SetActive(false);
        wrongImage.SetActive(false);

        ShowQuestion(0);
    }

    void ShowQuestion(int index)
    {
        // Hide all panels in the current set
        foreach (GameObject p in currentQuestionSet) p.SetActive(false);

        // Show only the requested index
        if (index < currentQuestionSet.Length)
        {
            currentQuestionSet[index].SetActive(true);
            SetButtonsInteractable(currentQuestionSet[index], true);
        }
        else
        {
            FinishQuiz();
        }
    }

    public void ProcessAnswer(bool isCorrect, GameObject panelObject)
    {
        if (isProcessing) return;
        StartCoroutine(HandleFeedback(isCorrect, panelObject));
    }

    IEnumerator HandleFeedback(bool isCorrect, GameObject panelObject)
    {
        isProcessing = true;

        SetButtonsInteractable(panelObject, false);

        if (isCorrect)
        {
            score++;
            correctImage.SetActive(true);
        }
        else
        {
            wrongImage.SetActive(true);
        }

        yield return new WaitForSeconds(2f); // Reduced to 2 seconds for faster testing (change back to 5 later)

        correctImage.SetActive(false);
        wrongImage.SetActive(false);

        questionIndex++;
        ShowQuestion(questionIndex);

        isProcessing = false;
    }

    void SetButtonsInteractable(GameObject panel, bool state)
    {
        Button[] btns = panel.GetComponentsInChildren<Button>();
        foreach (Button b in btns)
        {
            b.interactable = state;
        }
    }

    void FinishQuiz()
    {
        Debug.Log("Quiz Level " + currentLevelID + " Finished! Score: " + score);

        // --- TESTING SEQUENCE LOGIC ---
        if (testSequenceMode)
        {
            // If we just finished Level 1, go to Level 2
            if (currentLevelID == 1)
            {
                Debug.Log("Testing Mode: Moving to Level 2...");
                StartQuizLevel(2);
                return;
            }
            // If we just finished Level 2, go to Level 3
            else if (currentLevelID == 2)
            {
                Debug.Log("Testing Mode: Moving to Level 3...");
                StartQuizLevel(3);
                return;
            }
        }

        // If we finished Level 3 (or testing mode is off), go back to Map
        SceneManager.LoadScene("MapScreen");
    }
}