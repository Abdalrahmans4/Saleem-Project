using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizController : MonoBehaviour
{
    [Header("TESTING MODE")]
    public bool testSequenceMode = true;

    [Header("Level 1 Configuration")]
    public GameObject level1Parent;
    public GameObject[] level1Questions;

    [Header("Level 2 Configuration")]
    public GameObject level2Parent;
    public GameObject[] level2Questions;

    [Header("Level 3 Configuration")]
    public GameObject level3Parent;
    public GameObject[] level3Questions;

    [Header("Feedback Images")]
    public GameObject correctImage;
    public GameObject wrongImage;

    [Header("Rules")]
    public int maxWrongAnswers = 3; // If you get 3 or more wrong, you fail (but only after finishing)

    private GameObject[] currentQuestionSet;
    private int questionIndex = 0;
    private int score = 0;
    private int wrongAnswers = 0;
    private bool isProcessing = false;

    private int currentLevelID = 1;

    void Start()
    {
        int levelToLoad = 1;

        if (testSequenceMode)
        {
            levelToLoad = 1;
        }
        else
        {
            levelToLoad = PlayerPrefs.GetInt("QuizToLoad", 1);
        }

        StartQuizLevel(levelToLoad);
    }

    void StartQuizLevel(int levelID)
    {
        currentLevelID = levelID;
        questionIndex = 0;
        score = 0;
        wrongAnswers = 0; // Reset mistakes

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

        correctImage.SetActive(false);
        wrongImage.SetActive(false);

        ShowQuestion(0);
    }

    void ShowQuestion(int index)
    {
        // Hide all previous panels
        foreach (GameObject p in currentQuestionSet) p.SetActive(false);

        // Check if we still have questions left
        if (index < currentQuestionSet.Length)
        {
            currentQuestionSet[index].SetActive(true);
            SetButtonsInteractable(currentQuestionSet[index], true);
        }
        else
        {
            // --- UPDATED LOGIC ---
            // We finished all questions. NOW we check if we won or lost.
            if (wrongAnswers >= maxWrongAnswers)
            {
                Debug.Log("Finished Quiz, but too many wrong answers. Failed.");
                FinishQuiz(false); // Failed
            }
            else
            {
                Debug.Log("Finished Quiz successfully!");
                FinishQuiz(true); // Passed
            }
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
            wrongAnswers++;
            wrongImage.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        correctImage.SetActive(false);
        wrongImage.SetActive(false);

        // --- UPDATED LOGIC ---
        // Always go to the next question, never stop early!
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

    void FinishQuiz(bool isSuccess)
    {
        Debug.Log("Quiz Completed. Result: " + (isSuccess ? "Passed" : "Failed"));

        if (testSequenceMode)
        {
            if (isSuccess && currentLevelID < 3)
            {
                StartQuizLevel(currentLevelID + 1);
            }
            else
            {
                Debug.Log("Test Sequence Ended or Failed.");
            }
            return;
        }

        // --- REAL GAMEPLAY RETURN LOGIC ---

        string successKey = "";
        string failKey = "";
        int sceneToLoad = 0;

        // Configure Keys based on Level
        if (currentLevelID == 1)
        {
            successKey = "Level1Complete";
            failKey = "Level1Failed";
            sceneToLoad = 2; // Return to Play Ground Lvl1
        }
        else if (currentLevelID == 2)
        {
            successKey = "Level2Complete";
            failKey = "Level2Failed";
            sceneToLoad = 3; // Return to Class Room Lvl2
        }
        // Add Level 3 logic here if needed

        // Save the result note
        if (isSuccess)
        {
            PlayerPrefs.SetInt(successKey, 1);
            PlayerPrefs.DeleteKey(failKey);
        }
        else
        {
            PlayerPrefs.SetInt(failKey, 1);
            PlayerPrefs.DeleteKey(successKey);
        }

        PlayerPrefs.Save();

        // Load the gameplay scene
        SceneManager.LoadScene(sceneToLoad);
    }
}